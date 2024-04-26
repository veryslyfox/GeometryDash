// using System.Windows.Media;

// class Image
// {
//     public Image(ImageElement[,] elements)
//     {
//         Elements = elements;
//     }
//     public bool Write(string fileName, bool overwrite)
//     {
//         if(File.Exists(fileName) & !overwrite)
//         {
//             return false;
//         }
//         var file = File.Open(fileName, FileMode.OpenOrCreate);
//         foreach (var element in Elements)
//         {
//             switch (element.ElementType)
//             {
//                 case ImageElementType.Triangle:
//                 file.WriteByte(0);
//                 file.Write(element.Rectangle.X1);
//                 break;
//                 case ImageElementType.Circle:
//                 file.WriteByte(1);
//                 break;
//                 case ImageElementType.Rectangle:
//                 file.WriteByte(2);
//                 break;
//             }
//         }
//         return true;
//     }
//     public ImageElement[,] Elements { get; }
// }
// class ImageElement
// {
//     public ImageElement(Triangle triangle, byte r, byte g, byte b)
//     {
//         Triangle = triangle;
//         ElementType = ImageElementType.Triangle;
//         R = r;
//         G = g;
//         B = b;
//     }
//     public ImageElement(Circle circle, byte r, byte g, byte b)
//     {
//         Circle = circle;
//         ElementType = ImageElementType.Circle;
//         R = r;
//         G = g;
//         B = b;
//     }
//     public ImageElement(Rectangle rectangle, byte r, byte g, byte b)
//     {
//         Rectangle = rectangle;
//         ElementType = ImageElementType.Rectangle;
//         R = r;
//         G = g;
//         B = b;
//     }
//     public object GetValue()
//     {
//         switch (ElementType)
//         {
//             case ImageElementType.Triangle:
//                 return Triangle;
//             case ImageElementType.Circle:
//                 return Circle;
//             case ImageElementType.Rectangle:
//                 return Rectangle;
//         }
//         throw new Exception("Don't use reflection for heck");
//     }
//     public Triangle Triangle { get; }
//     public Circle Circle { get; }
//     public Rectangle Rectangle { get; }
//     public byte R { get; }
//     public byte G { get; }
//     public byte B { get; }
//     public ImageElementType ElementType;
// }
// enum ImageElementType
// {
//     Triangle,
//     Circle,
//     Rectangle,
// }