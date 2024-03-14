using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using SmartRefund.Application.Interfaces;
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Models;
using SmartRefund.Domain.Models.Enums;
using SmartRefund.Infra.Interfaces;
using SmartRefund.ViewModels.Responses;
using System.Text.RegularExpressions;

public class VisionExecutorService : IVisionExecutorService
{
    private IInternalReceiptRepository _internalReceiptRepository;
    private IRawVisionReceiptRepository _rawVisionReceiptRepository;
    private ILogger<VisionExecutorService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IVisionExecutorServiceConfiguration _visionConfig;

    public VisionExecutorService(
        IInternalReceiptRepository repository,
        IRawVisionReceiptRepository rawVisionReceiptRepository,
        ILogger<VisionExecutorService> logger,
        IConfiguration configuration,
        IVisionExecutorServiceConfiguration visionConfig)
    {
        _internalReceiptRepository = repository;
        _rawVisionReceiptRepository = rawVisionReceiptRepository;
        _logger = logger;
        _configuration = configuration;
        _visionConfig = visionConfig;
    }

    public async Task<RawVisionReceipt> ExecuteRequestAsync(InternalReceipt input)
    {
        if (!IsExecutableStatus(input.Status))
            throw new NonVisionExecutableStatus(input.Id, input.Status.ToString());

        OpenAIAPI api = ConfigureApiKey();
        var rawImage = input.Image;
        var conversation = api.Chat.CreateConversation();
        conversation.Model = Model.GPT4_Vision;
        var response = await ProcessVisionResponseAsync(conversation, rawImage, input);
        var addedRawVisionReceipt = await CreateRawVisionReceiptAsync(input, response);
        await UpdateInternalReceiptAsync(input);

        return addedRawVisionReceipt;
    }

    public async Task<RawVisionResponse> ProcessVisionResponseAsync(Conversation conversation, byte[] rawImage, InternalReceipt input)
    {
        var response = new RawVisionResponse();
        var prompts = _visionConfig.OpenAIVisionPrompts;

        conversation.AppendSystemMessage(prompts.SystemPrompt);
        conversation.AppendUserInput(prompts.ImagePrompt, ChatMessage.ImageInput.FromImageBytes(rawImage));

        response.IsReceipt = await GetResponseAsync(conversation, prompts.IsReceiptPrompt, new NonReceiptException(input.Id));
        response.Total = await GetResponseAsync(conversation, prompts.TotalPrompt);
        response.Category = await GetResponseAsync(conversation, prompts.CategoryPrompt);
        response.Description = await GetResponseAsync(conversation, prompts.DescriptionPrompt);

        return response;
    }

    public async Task<string> GetResponseAsync(Conversation conversation, string prompt, Exception? exceptionIfInvalid = null)
    {
        conversation.AppendUserInput(prompt);
        var answer = await conversation.GetResponseFromChatbotAsync();

        if (exceptionIfInvalid != null && IsInvalidAnswer(answer))
            throw exceptionIfInvalid;

        return answer;
    }

    public bool IsInvalidAnswer(string answer)
    {
        return Regex.IsMatch(answer, @"\bn(ã|a)o\b", RegexOptions.IgnoreCase);
    }

    public async Task<RawVisionReceipt> CreateRawVisionReceiptAsync(InternalReceipt receipt, RawVisionResponse response)
    {
        var rawVisionReceipt = new RawVisionReceipt(receipt, isReceipt: response.IsReceipt, category: response.Category, total: response.Total, description: response.Description);
        var addedRawVisionReceipt = await _rawVisionReceiptRepository.AddAsync(rawVisionReceipt);
        _logger.LogInformation($"Internal Receipt was interpreted by GPT Vision and added to repository (Id: {addedRawVisionReceipt.Id})");
        return addedRawVisionReceipt;
    }

    public async Task<InternalReceipt> UpdateInternalReceiptAsync(InternalReceipt input)
    {
        input.SetStatus(InternalReceiptStatusEnum.Successful);
        var updateInternalReceipt = await _internalReceiptRepository.UpdateAsync(input);
        _logger.LogInformation($"Internal Receipt status updated to {updateInternalReceipt.Status} (Id: {updateInternalReceipt.Id})");
        return updateInternalReceipt;
    }

    public OpenAIAPI ConfigureApiKey()
    {
        var envVar = _visionConfig.OpenAIVisionApiEnvVar;
        var apiKey = _configuration[envVar];

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ApiKeyNotFoundException(envVar);

        return new OpenAIAPI(apiKey);
    }

    public bool IsExecutableStatus(InternalReceiptStatusEnum status)
    {
        return status != InternalReceiptStatusEnum.Successful
            && status != InternalReceiptStatusEnum.Unsuccessful;
    }

    public async Task<IEnumerable<InternalReceipt>> GetInternalReceiptsWithStatusAsync(InternalReceiptStatusEnum status)
    {
        return await _internalReceiptRepository.GetByStatusAsync(status);
    }
}
