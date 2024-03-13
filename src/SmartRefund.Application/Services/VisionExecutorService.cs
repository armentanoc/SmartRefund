using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartRefund.Infra.Interfaces;
using SmartRefund.Application.Interfaces;
using SmartRefund.Domain.Models.Enums;
using SmartRefund.Domain.Models;
using OpenAI_API.Models;
using OpenAI_API.Chat;
using OpenAI_API;
using SmartRefund.CustomExceptions;

namespace SmartRefund.Application.Services
{
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
        }

        public async Task<RawVisionReceipt> ExecuteRequest(InternalReceipt input)
        {
            if (!IsExecutableStatus(input.Status))
                throw new NonVisionExecutableStatus(input.Id, input.Status.ToString()); 

            OpenAIAPI api = ConfigureApiKey();
            var rawImage = input.Image;
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
                internalReceipt: input,
                isReceipt: isReceiptAnswer,
                total: totalAnswer,
                category: categoryAnswer,
                description: descriptionAnswer
                );

            var addedRawVisionReceipt = await _rawVisionReceiptRepository.AddAsync(rawVisionReceipt);
            _logger.LogInformation($"Internal Receipt was interpreted by GPT Vision and added to repository (Id {addedRawVisionReceipt.Id})");

            input.SetStatus(InternalReceiptStatusEnum.Successful);
            var updateInternalReceipt = await _internalReceiptRepository.UpdateAsync(input);
            _logger.LogInformation($"Internal Receipt status updated to {updateInternalReceipt.Status} (Id {updateInternalReceipt.Id})");

            return addedRawVisionReceipt;
        }

        public OpenAIAPI ConfigureApiKey()
        {
            var envVariableApiKey = _configuration.GetSection("OpenAIVisionConfig:EnvVariable").Value;

            if (string.IsNullOrWhiteSpace(envVariableApiKey))
                throw new ApiKeyNotFoundException();

            var APIKey = _configuration[envVariableApiKey];

            if (string.IsNullOrWhiteSpace(APIKey))
                throw new ApiKeyNotFoundException(envVariableApiKey);

            return new OpenAIAPI(APIKey);
        }

        public bool IsExecutableStatus(InternalReceiptStatusEnum status)
        {
            return status != InternalReceiptStatusEnum.Successful
                && status != InternalReceiptStatusEnum.Unsuccessful;
        }
    }
}