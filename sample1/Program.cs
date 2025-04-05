namespace sample1;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        var p1 = new Personne("Thibault", 23);
        string output = JsonConvert.SerializeObject(p1, Formatting.Indented);
        Console.WriteLine(output);

        var watch = Stopwatch.StartNew();
        Resize();
        watch.Stop();

        var watchForParallel = Stopwatch.StartNew();
        ParallelResize();
        watchForParallel.Stop();

        Console.WriteLine($"Classical foreach loop | Time Taken : {watch.ElapsedMilliseconds} ms.");
        Console.WriteLine($"Parallel.ForEach loop  | Time Taken : {watchForParallel.ElapsedMilliseconds} ms.");
    }

    private static void Resize()
    {
        using (Image image = Image.Load("input/imageTest.png"))
        {
            image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));

            image.Save("output/imageTest-resized.png");
        }

        using (Image image = Image.Load("input/imageTest2.png"))
        {
            image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));

            image.Save("output/imageTest2-resized.png");
        }
    }

    private static void ParallelResize()
    {
        Parallel.ForEach(["input/imageTest.png", "input/imageTest2.png"], imageName =>
        {
            using (Image image = Image.Load(imageName))
            {
                image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));

                image.Save("output/" + imageName.Split("/")[1].Split(".")[0] + "-parallel-resized.png");
            }
        }
        );
    }
}
