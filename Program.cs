//Written for games on 001 Game Creator. https://001gamecreator.com/

using System.IO.Compression;

BinaryReader br = new(File.OpenRead(args[0]));
Directory.CreateDirectory(Path.GetDirectoryName(args[0]) + "\\" + Path.GetFileNameWithoutExtension(args[0]));
br.BaseStream.Position += 8;
int fileCount = br.ReadInt32();
for (int i = 0; i < fileCount; i++)
{
    string name = new(br.ReadChars(br.ReadInt32()));
    int sizeUncompressed = br.ReadInt32();
    int size = br.ReadInt32();

    using FileStream FS = File.Create(Path.GetDirectoryName(args[0]) + "//" + Path.GetFileNameWithoutExtension(args[0]) + "//" + name);
    if (sizeUncompressed == -1)
    {
        BinaryWriter bw = new(FS);
        bw.Write(br.ReadBytes(size));
    }
    else
    {
        br.ReadInt16();
        using (var ds = new DeflateStream(new MemoryStream(br.ReadBytes(size - 2)), CompressionMode.Decompress))
            ds.CopyTo(FS);
    }

    FS.Close();
}