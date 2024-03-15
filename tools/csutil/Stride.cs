//using System;

//namespace Scaffolding.Base;

//public interface IStride<T>
//{
//  T[] Buffer { get; }
//  int Dimensions { get; }
//}

//public struct Stride1<T> : IStride<T>
//{
//  public T[] Buffer { get; set; }
//  public int Length { get; set; }
//  public int Stride { get; set; }
//  public int Offset { get; set; }
//  public int Dimensions => 1;

//  public Stride1(T[] buffer, int length, int stride, int offset)
//  {
//    Buffer = buffer;
//    Length = length;
//    Stride = stride;
//    Offset = offset;
//  }

//  public static Stride1<T> View(T[] buffer)
//    => new Stride1<T>(buffer, buffer.Length, 1, 0);

//  private int Index(int i)
//    => Offset + Stride * (i >= 0 ? i : Math.Max(0, Length + i));
//  public T Get(int i) => Buffer[Index(i)];
//  public T Set(int i, T v) => Buffer[Index(i)] = v;
//  public T this[int i] { get { return Get(i); } set { Set(i, value); } }

//  public Stride1<T> Low(int i)
//  {
//    int o = Offset + Stride * i; Length -= i;
//    return new Stride1<T>(Buffer, Length, Stride, o);
//  }
//  public Stride1<T> High(int i)
//  {
//    return new Stride1<T>(Buffer, i, Stride, Offset);
//  }
//  public Stride1<T> Step(int c)
//  {
//    if (c < 2) return this;
//    int l = (int)Math.Ceiling(Length / (double)c);
//    int s = Stride * c;
//    return new Stride1<T>(Buffer, l, s, Offset);
//  }
//  public Stride1<T> Flip()
//  {
//    int o = Offset + Stride * (Length - 1);
//    int s = -Stride;
//    return new Stride1<T>(Buffer, Length, s, o);
//  }
//}

//public struct Stride2<T> : IStride<T>
//{
//  public T[] Buffer { get; set; }
//  public int Height { get; set; }
//  public int StrideHeight { get; set; }
//  public int Width { get; set; }
//  public int StrideWidth { get; set; }
//  public int Offset { get; set; }
//  public int Dimensions => 2;

//  public Stride2(T[] buffer,
//    int height, int strideHeight,
//    int width, int strideWidth,
//    int offset)
//  {
//    Buffer = buffer;
//    Height = height;
//    StrideHeight = strideHeight;
//    Width = width;
//    StrideWidth = strideWidth;
//    Offset = offset;
//  }

//  public static Stride2<T> View(T[] buffer, int height, int width)
//    => new Stride2<T>(buffer, height, height, width, 1, 0);

//  private int Index(int y, int x)
//    => Offset
//    + (StrideHeight * (y >= 0 ? y : Math.Max(0, Height + y)))
//    + (StrideWidth * (x >= 0 ? x : Math.Max(0, Width + x)));
//  public T Get(int y, int x) => Buffer[Index(y, x)];
//  public T Set(int y, int x, T v) => Buffer[Index(y, x)] = v;
//  public T this[int y, int x] { get { return Get(y, x); } set { Set(y, x, value); } }

//  public Stride2<T> Low(int y, int x)
//  {
//    int h = Height - y;
//    int w = Width - x;
//    int o = Offset + (StrideHeight * y) + (StrideWidth * x);
//    return new Stride2<T>(Buffer, h, StrideHeight, w, StrideWidth, o);
//  }
//  public Stride2<T> High(int y, int x)
//  {
//    return new Stride2<T>(Buffer, y, StrideHeight, x, StrideWidth, 0);
//  }
//  public Stride2<T> StepY(int c)
//  {
//    if (c < 2) return this;
//    int h = (int)Math.Ceiling(Height / (double)c);
//    int s = Height * c;
//    return new Stride2<T>(Buffer, h, s, Width, StrideWidth, Offset);
//  }
//  public Stride2<T> StepX(int c)
//  {
//    if (c < 2) return this;
//    int w = (int)Math.Ceiling(Width / (double)c);
//    int s = Width * c;
//    return new Stride2<T>(Buffer, Height, StrideHeight, w, s, Offset);
//  }
//  public Stride2<T> FlipY()
//  {
//    int o = Offset + StrideHeight * (Height - 1);
//    int s = -StrideHeight;
//    return new Stride2<T>(Buffer, Height, s, Width, StrideWidth, o);
//  }
//  public Stride2<T> FlipX()
//  {
//    int o = Offset + StrideWidth * (Width - 1);
//    int s = -StrideWidth;
//    return new Stride2<T>(Buffer, Height, StrideHeight, Width, s, o);
//  }
//  public Stride2<T> Pose()
//  {
//    return new Stride2<T>(Buffer, Width, StrideWidth, Height, StrideHeight, Offset);
//  }
//  public Stride1<T> ToY(int x)
//  {
//    int o = Offset + StrideHeight * x;
//    return new Stride1<T>(Buffer, Height, StrideHeight, o);
//  }
//  public Stride1<T> ToX(int y)
//  {
//    int o = Offset + StrideWidth * y;
//    return new Stride1<T>(Buffer, Width, StrideWidth, o);
//  }
//}

//public struct Stride3<T> : IStride<T>
//{
//  public T[] Buffer { get; set; }
//  public int Height { get; set; }
//  public int StrideHeight { get; set; }
//  public int Width { get; set; }
//  public int StrideWidth { get; set; }
//  public int Depth { get; set; }
//  public int StrideDepth { get; set; }
//  public int Offset { get; set; }
//  public int Dimensions => 3;

//  public Stride3(T[] buffer,
//    int height, int strideHeight,
//    int width, int strideWidth,
//    int depth, int strideDepth,
//    int offset)
//  {
//    Buffer = buffer;
//    Height = height;
//    StrideHeight = strideHeight;
//    Width = width;
//    StrideWidth = strideWidth;
//    Depth = depth;
//    StrideDepth = strideDepth;
//    Offset = offset;
//  }

//  public static Stride3<T> View(T[] buffer, int height, int width, int depth)
//    => new Stride3<T>(buffer, height, width * depth, width, depth, depth, 1, 0);

//  private int Index(int y, int x, int z)
//    => Offset
//    + (StrideHeight * (y >= 0 ? y : Math.Max(0, Height + y)))
//    + (StrideWidth * (x >= 0 ? x : Math.Max(0, Width + x)))
//    + (StrideDepth * (z >= 0 ? z : Math.Max(0, Depth + z)));
//  public T Get(int y, int x, int z) => Buffer[Index(y, x, z)];
//  public T Set(int y, int x, int z, T v) => Buffer[Index(y, x, z)] = v;
//  public T this[int y, int x, int z] { get { return Get(y, x, z); } set { Set(y, x, z, value); } }

//  public Stride3<T> Low(int y, int x, int z)
//  {
//    throw new NotImplementedException();
//  }
//  public Stride3<T> High(int y, int x, int z)
//  {
//    throw new NotImplementedException();
//  }
//  public Stride3<T> StepY(int c)
//  {
//    throw new NotImplementedException();
//  }
//  public Stride3<T> StepX(int c)
//  {
//    throw new NotImplementedException();
//  }
//  public Stride3<T> StepZ(int c)
//  {
//    throw new NotImplementedException();
//  }
//  public Stride3<T> FlipY()
//  {
//    throw new NotImplementedException();
//  }
//  public Stride3<T> FlipX()
//  {
//    throw new NotImplementedException();
//  }
//  public Stride3<T> FlipZ()
//  {
//    throw new NotImplementedException();
//  }
//  public Stride3<T> PoseXZ()
//  {
//    throw new NotImplementedException();
//  }
//  public Stride3<T> PoseYZ()
//  {
//    throw new NotImplementedException();
//  }
//  public Stride3<T> PoseYX()
//  {
//    throw new NotImplementedException();
//  }
//  public Stride2<T> ToXZ(int x)
//  {
//    throw new NotImplementedException();
//  }
//  public Stride2<T> ToYZ(int y)
//  {
//    throw new NotImplementedException();
//  }
//  public Stride2<T> ToYX(int y)
//  {
//    throw new NotImplementedException();
//  }
//  public Stride1<T> ToY(int x, int z)
//  {
//    throw new NotImplementedException();
//  }
//  public Stride1<T> ToX(int y, int z)
//  {
//    throw new NotImplementedException();
//  }
//  public Stride1<T> ToZ(int y, int x)
//  {
//    throw new NotImplementedException();
//  }
//}
