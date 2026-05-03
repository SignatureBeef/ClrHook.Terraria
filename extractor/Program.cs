
Console.WriteLine("Embedded Resource Extractor");

if (args.Length < 2)
{
    Console.WriteLine("Usage: extractor <input-assembly> <output-dir>");
    return;
}

var input = args[0];
var output = args[1];

Directory.CreateDirectory(output);

var asm = Assembly.LoadFrom(input);

foreach (var res in asm.GetManifestResourceNames())
{
    if (!res.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
        continue;

    using var stream = asm.GetManifestResourceStream(res);
    if (stream == null) continue;

    var fileName = res.Split('.').Reverse().Skip(1).First();
    var path = Path.Combine(output, fileName + ".dll");

    using (var fs = File.Create(path))
        stream.CopyTo(fs);

    Console.WriteLine($"Extracted embedded resource: {fileName}");
}
