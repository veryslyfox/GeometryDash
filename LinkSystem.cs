// class Particle
// {
//     public Particle(Particle[] linkedParticles, int x, int y)
//     {
//         LinkedParticles = linkedParticles;
//         X = x;
//         Y = y;
//     }

//     public Particle[] LinkedParticles { get; }
//     public int X { get; }
//     public int Y { get; }
//     public int DX { get; set; }
//     public int DY { get; set; }
//     public int A { get; }
//     public int B { get; }
//     public void Tick()
//     {
//         foreach (var p in LinkedParticles)
//         {
//             var dx = p.X - X;
//             var dy = p.Y - Y;
//             var distance = (dx * dx + dy * dy);
//             var dd = distance * A + B;
//             DX = dx * dd;
//             DY = dy * dd;
//         }
//     }
//     public void Tick2()
//     {
//         X -= DX;   
//         X -= DY;   
//     }
// }