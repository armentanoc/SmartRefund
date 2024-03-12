using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models;
using SmartRefund.Infra.Interfaces;
using OpenAI_API.Chat;
using OpenAI_API;
using OpenAI_API.Models;
using SmartRefund.Domain.Models.Enums;


namespace SmartRefund.Application.Services
{
    //pegar do banco em []bytes -> converter pra base 64 -> enviar pra api da openai -> pegar o retorno -> salvar no banco
    public class VisionExecutorService : IVisionExecutorService
    {
        private IInternalReceiptRepository _internalReceiptRepository;
        private IRawVisionReceiptRepository _rawVisionReceiptRepository;
        private ILogger<VisionExecutorService> _logger;
        private IConfiguration _configuration;

        public VisionExecutorService(IInternalReceiptRepository repository, IRawVisionReceiptRepository rawVisionReceiptRepository, ILogger<VisionExecutorService> logger, IConfiguration configuration)
        {
            _internalReceiptRepository = repository;
            _rawVisionReceiptRepository = rawVisionReceiptRepository;
            _logger = logger;
            _configuration = configuration;
            //é o mesmo configuration tanto appsettings.json quanto env variables, inclusive faz hierarquia de sobreposição
        }

        public async Task<RawVisionReceipt> Start(InternalReceipt internalReceiptInput, string apiKey)
        {
            if (internalReceiptInput.Status.Equals(InternalReceiptStatusEnum.Successful) ||
                internalReceiptInput.Status.Equals(InternalReceiptStatusEnum.Unsuccessful))
                throw new Exception("Já processou"); //criar exceção personalizdada

            var APIkey = apiKey; //vir do environment variables
            //: não é um caractere válido para variáveis de ambiente, temos que substituir por __ em caso de hierarquia
            var rawImage = internalReceiptInput.Image;

            OpenAIAPI api = new OpenAIAPI(APIkey);
            var chat = api.Chat.CreateConversation();
            chat.Model = Model.GPT4_Vision;

            chat.AppendSystemMessage("Você é um especialista em ler notas fiscais e extrair informações importantes.");

            chat.AppendUserInput("Você deve considerar essa imagem de nota fiscal para responder às próximas perguntas", ChatMessage.ImageInput.FromImageBytes(rawImage));

            chat.AppendUserInput("Essa imagem é algum comprovante fiscal? Responda com SIM ou NAO");
            string isReceiptAnswer = await chat.GetResponseFromChatbotAsync();

            chat.AppendUserInput("Qual o valor total dessa despesa? Escreva o valor apenas com números.");
            string totalAnswer = await chat.GetResponseFromChatbotAsync();

            chat.AppendUserInput("Que categoria de despesa é essa? Responda entre: HOSPEDAGEM OU TRANSPORTE OU VIAGEM OU ALIMENTAÇÃO OU OUTROS.");
            string categoryAnswer = await chat.GetResponseFromChatbotAsync();

            chat.AppendUserInput("Retorne uma descrição da nota fiscal em texto corrido (sem tópicos e apenas um parágrafo) apenas contendo: Nome da Empresa; CNPJ (caso possua); Data e horário da emissão da nota; Detalhamento dos itens");
            string descriptionAnswer = await chat.GetResponseFromChatbotAsync();

            //compor um prompt só para respostas, para que seja estritamente objetivo, separado por X, enviar lista, etc.
            //passar um exemplo, etc. 

            var rawVisionReceipt = new RawVisionReceipt(
                internalReceipt: internalReceiptInput,
                isReceipt: isReceiptAnswer,
                total: totalAnswer,
                category: categoryAnswer,
                description: descriptionAnswer
                );

            var addedRawVisionReceipt = await _rawVisionReceiptRepository.AddAsync(rawVisionReceipt);
            _logger.LogInformation($"Internal Receipt was interpreted by GPT Vision and added to repository (Id {addedRawVisionReceipt.Id})");

            internalReceiptInput.SetStatus(Domain.Models.Enums.InternalReceiptStatusEnum.Successful);
            var updateInternalReceipt = await _internalReceiptRepository.UpdateAsync(internalReceiptInput);
            _logger.LogInformation($"Internal Receipt status updated to {updateInternalReceipt.Status} (Id {updateInternalReceipt.Id})");

            return addedRawVisionReceipt;
        }
    }
}