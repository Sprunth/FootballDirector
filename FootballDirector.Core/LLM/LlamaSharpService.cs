using LLama;
using LLama.Common;
using LLama.Native;
using LLama.Sampling;

namespace FootballDirector.Core.LLM;

/// <summary>
/// LLM service implementation using LLamaSharp for local model inference.
/// </summary>
public sealed class LlamaSharpService : ILlmService
{
    private static bool _loggingInitialized;
    private readonly string _modelPath;
    private readonly object _lock = new();
    private LLamaWeights? _model;
    private LLamaContext? _context;
    private bool _disposed;

    /// <summary>
    /// Creates a new LlamaSharpService instance.
    /// </summary>
    /// <param name="modelPath">Path to the GGUF model file.</param>
    public LlamaSharpService(string modelPath)
    {
        _modelPath = modelPath ?? throw new ArgumentNullException(nameof(modelPath));
        EnableNativeLogging();
    }

    private static void EnableNativeLogging()
    {
        if (_loggingInitialized) return;
        _loggingInitialized = true;

        NativeLibraryConfig.All.WithLogCallback((level, message) =>
        {
            Console.WriteLine($"[LLamaSharp/{level}] {message}");
        });
    }

    /// <inheritdoc />
    public bool IsAvailable => File.Exists(_modelPath);

    /// <inheritdoc />
    public async Task<LlmResponse> GenerateAsync(LlmRequest request, CancellationToken ct = default)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(LlamaSharpService));

        if (!IsAvailable)
            return new LlmResponse(string.Empty, false, $"Model file not found: {_modelPath}");

        try
        {
            EnsureModelLoaded();

            var prompt = BuildPrompt(request);
            var inferenceParams = new InferenceParams
            {
                MaxTokens = request.MaxTokens,
                SamplingPipeline = new DefaultSamplingPipeline
                {
                    Temperature = request.Temperature
                },
                AntiPrompts = ["<|im_end|>", "<|end|>", "</s>"]
            };

            var executor = new InteractiveExecutor(_context!);
            var result = new System.Text.StringBuilder();

            await foreach (var token in executor.InferAsync(prompt, inferenceParams, ct))
            {
                result.Append(token);
            }

            return new LlmResponse(result.ToString().Trim(), true);
        }
        catch (OperationCanceledException)
        {
            return new LlmResponse(string.Empty, false, "Generation was cancelled");
        }
        catch (Exception ex)
        {
            return new LlmResponse(string.Empty, false, ex.Message);
        }
    }

    private void EnsureModelLoaded()
    {
        if (_model is not null)
            return;

        lock (_lock)
        {
            if (_model is not null)
                return;

            // Log available devices
            var deviceCount = NativeApi.llama_max_devices();
            Console.WriteLine($"[LLM] Max devices available: {deviceCount}");

            var parameters = new ModelParams(_modelPath)
            {
                ContextSize = 2048,
                GpuLayerCount = -1, // -1 = offload ALL layers to GPU
                MainGpu = 0         // Use first GPU (your 4090)
            };

            Console.WriteLine($"[LLM] Loading model with GpuLayerCount={parameters.GpuLayerCount}, MainGpu={parameters.MainGpu}");

            _model = LLamaWeights.LoadFromFile(parameters);
            _context = _model.CreateContext(parameters);

            Console.WriteLine("[LLM] Model loaded successfully");
        }
    }

    private static string BuildPrompt(LlmRequest request)
    {
        if (string.IsNullOrEmpty(request.SystemPrompt))
        {
            return $"<|im_start|>user\n{request.Prompt}<|im_end|>\n<|im_start|>assistant\n";
        }

        return $"<|im_start|>system\n{request.SystemPrompt}<|im_end|>\n<|im_start|>user\n{request.Prompt}<|im_end|>\n<|im_start|>assistant\n";
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;
        _context?.Dispose();
        _model?.Dispose();

        await Task.CompletedTask;
    }
}
