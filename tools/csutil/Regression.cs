//using System;

//namespace My.Core
//{
//  /// <summary>
//  /// A regression perceptron using one hidden layer and sigmoid activation
//  /// The implementation is based on https://github.com/glouw/tinn.
//  /// </summary>
//  public class StdTinn
//  {
//    public const float DefaultLearningRate = 0.01f;

//    private static IStdRng _rng = new StdUniformRng();

//    public int InputCount { get; set; }
//    public float[] InputWeights { get; set; }
//    public float InputBias { get; set; }
//    public int HiddenCount { get; set; }
//    public float[] HiddenWeights { get; set; }
//    public float HiddenBias { get; set; }
//    public float[] HiddenSignals { get; set; }
//    public int OutputCount { get; set; }
//    public float[] OutputSignals { get; set; }

//    public StdTinn(int inputCount, int outputCount, int? hiddenCount = null, IStdRng rng = null)
//    {
//      // Initialize properties
//      InputCount = inputCount;
//      HiddenCount = hiddenCount ?? InputCount;
//      OutputCount = outputCount;

//      InputWeights = new float[InputCount * HiddenCount];
//      HiddenWeights = new float[HiddenCount * OutputCount];

//      HiddenSignals = new float[HiddenCount];
//      OutputSignals = new float[OutputCount];

//      // Randomize weights and biases
//      rng = rng ?? _rng;

//      for (int i = 0; i < InputWeights.Length; i++)
//        InputWeights[i] = (float)rng.Next() - 0.5f;

//      for (int i = 0; i < HiddenWeights.Length; i++)
//        HiddenWeights[i] = (float)rng.Next() - 0.5f;

//      InputBias = (float)rng.Next() - 0.5f;
//      HiddenBias = (float)rng.Next() - 0.5f;
//    }

//    public float Adapt(float[] inputs, float[] targets,
//      float learningRate = DefaultLearningRate)
//    {
//      PropagateForward(inputs);
//      PropagateBackward(inputs, targets, learningRate);
//      return ComputeErrorTotal(targets);
//    }

//    public float[] Predict(float[] inputs)
//    {
//      PropagateForward(inputs);
//      return OutputSignals;
//    }

//    public void PropagateForward(float[] inputs)
//    {
//      for (int i = 0; i < HiddenCount; i++)
//      {
//        float sum = 0.0f;
//        for (int j = 0; j < InputCount; j++)
//        {
//          float signal = inputs[j];
//          float weight = InputWeights[i * InputCount + j];
//          sum += signal * weight;
//        }
//        HiddenSignals[i] = ComputeActivation(sum + InputBias);
//      }
//      for (int i = 0; i < OutputCount; i++)
//      {
//        float sum = 0.0f;
//        for (int j = 0; j < HiddenCount; j++)
//        {
//          float signal = HiddenSignals[j];
//          float weight = HiddenWeights[i * HiddenCount + j];
//          sum += signal * weight;
//        }
//        OutputSignals[i] = ComputeActivation(sum + HiddenBias);
//      }
//    }

//    public void PropagateBackward(float[] inputs, float[] targets,
//      float learningRate = DefaultLearningRate)
//    {
//      for (int i = 0; i < HiddenCount; i++)
//      {
//        float sum = 0.0f;
//        for (int j = 0; j < OutputCount; j++)
//        {
//          float a = ComputeErrorPartialDerivative(OutputSignals[j], targets[j]);
//          float b = ComputeActivationPartialDerivative(OutputSignals[j]);
//          sum += a * b * HiddenWeights[j * HiddenCount + i];
//          HiddenWeights[j * HiddenCount + i] -= learningRate * a * b * HiddenSignals[i];
//        }
//        for (int j = 0; j < InputCount; j++)
//          InputWeights[i * InputCount + j] -= learningRate * sum * ComputeActivationPartialDerivative(HiddenSignals[i]) * inputs[j];
//      }
//    }

//    /// <summary>
//    /// Sigmoid activation
//    /// </summary>
//    private float ComputeActivation(float a)
//    {
//      return 1.0f / (1.0f + (float)Math.Exp(-a));
//    }

//    /// <summary>
//    /// Sigmoid activation (partial derivative)
//    /// </summary>
//    private float ComputeActivationPartialDerivative(float a)
//    {
//      return a * (1.0f - a);
//    }

//    /// <summary>
//    /// Mean square error
//    /// </summary>
//    private float ComputeError(float a, float b)
//    {
//      return 0.5f * (a - b) * (a - b);
//    }

//    /// <summary>
//    /// Mean square error (partial derivative)
//    /// </summary>
//    private float ComputeErrorPartialDerivative(float a, float b)
//    {
//      return a - b;
//    }

//    /// <summary>
//    /// Mean square error (total)
//    /// </summary>
//    private float ComputeErrorTotal(float[] targets)
//    {
//      float sum = 0.0f;
//      for (int i = 0; i < OutputCount; i++)
//      {
//        float target = targets[i];
//        float signal = OutputSignals[i];
//        sum += ComputeError(target, signal);
//      }
//      return sum;
//    }
//  }

//  #region [Trainers]

//  public interface IVgTinnTrainer
//  {
//  }

//  /// <summary>
//  /// A model optimizer using mean square error loss
//  /// </summary>
//  public class VgTinnTrainer : IVgTinnTrainer
//  {
//  }

//  #endregion

//  #region [Datasets]

//  public interface IVgTinnDataset
//  {
//  }

//  /// <summary>
//  /// A memory dataset with support for shuffling and batching
//  /// </summary>
//  public class VgTinnDataset : IVgTinnDataset
//  {
//  }

//  #endregion
//}
