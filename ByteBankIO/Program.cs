using ByteBankIO.Models;
using System.Text;

Console.Clear();



#region Etapas...
static void UsandoBuffer()
{
    var path = "contas.txt";

    using (var stream = new FileStream(path, FileMode.Open))
    {
        var buffer = new byte[1024]; // 1Kb

        var bytesLidos = -1;
        while (bytesLidos != 0)
        {
            // preenche o buffer com os dados da posição 0 até 1024
            bytesLidos = stream.Read(buffer, 0, 1024);

            //Console.WriteLine($"Bytes Lidos: {bytesLidos}");
            WriteBuffer(buffer, bytesLidos);
        }

        stream.Close();
    }

    static void WriteBuffer(byte[] buffer, int bytesLidos)
    {
        var utf8 = new UTF8Encoding();

        var contas = utf8.GetString(buffer, 0, bytesLidos);
        Console.WriteLine(contas);
    }
}

static void UsandoReaderStream()
{
    var path = "contas.txt";

    using (var stream = new FileStream(path, FileMode.Open))
    {
        var reader = new StreamReader(stream);

        //var result = reader.ReadToEnd();
        //var result = reader.ReadLine();
        //Console.WriteLine(result);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            Console.WriteLine(line);
        }
    }
}

static void CriarContaUsandoOArquivo()
{
    var path = "contas.txt";

    using (var stream = new FileStream(path, FileMode.Open))
    {
        var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine()!;
            var conta = StringToContaCorrente(line);

            string message = $"Titular: {conta.Titular.Nome.PadRight(10)}\tAgencia: {conta.Agencia}\tConta: {conta.Numero}\tSaldo: {conta.Saldo:C}";
            Console.WriteLine(message);
        }
    }


    static ContaCorrente StringToContaCorrente(string line)
    {
        var campos = line.Split(' ');

        var agencia = int.Parse(campos[0]);
        var numero = int.Parse(campos[1]);
        var saldo = double.Parse(campos[2]);
        var titular = new Cliente();
        titular.Nome = campos[3];

        var resultado = new ContaCorrente(agencia, numero);
        resultado.Titular = titular;
        resultado.Depositar(saldo);

        return resultado;
    }
}

static void CriarArquivo()
{
    var path = "contasExportadas.csv";

    using (var stream = new FileStream(path, FileMode.Create))
    {
        var conta = "456,7895,4785.40,Gustavo Santos";

        var encoding = Encoding.UTF8;
        var bytes = encoding.GetBytes(conta);

        stream.Write(bytes, 0, bytes.Length);
    }
}

static void CriaArquivoComStreamWriter()
{
    var path = "contasExportadas.csv";

    using (var stream = new FileStream(path, FileMode.Create))
    using (var writer = new StreamWriter(stream))
    {
        var conta = "456,7895,4785.40,Gustavo Ribas";

        writer.Write(conta);
    }
}

static void CriaArquivoComFlush()
{
    var caminhoArquivo = "teste.txt";

    using (var fluxoDeArquivo = new FileStream(caminhoArquivo, FileMode.Create))
    using (var escritor = new StreamWriter(fluxoDeArquivo))
    {
        for (int i = 0; i < 100000000; i++)
        {
            escritor.WriteLine($"Linha {i}");
            escritor.Flush();
            //Console.WriteLine($"Linha {i} foi escrita no arquivo. Tecle enter p adicionar mais uma!");
            Console.ReadLine();
        }
    }
}

static void EscritaBinaria()
{
    using (var fs = new FileStream("contaCorrente.txt", FileMode.Create))
    using (var escritor = new BinaryWriter(fs))
    {
        escritor.Write(456); // Número da Agência
        escritor.Write(546544); // Número da conta
        escritor.Write(4000.50); // Saldo
        escritor.Write("Gustavo Braga");
    }
}

static void LeituraBinaria()
{
    using (var fs = new FileStream("contaCorrente.txt", FileMode.Open))
    using (var leitor = new BinaryReader(fs))
    {
        var agencia = leitor.ReadInt32();
        var numeroConta = leitor.ReadInt32();
        var saldo = leitor.ReadDouble();
        var titular = leitor.ReadString();

        Console.WriteLine($"{agencia}/{numeroConta} {titular} {saldo}");
    }
}

static void UsandoInputDoConsole()
{
    using (var stream = Console.OpenStandardInput())
    using (var fs = new FileStream("entradaConsole.txt", FileMode.Create))
    {
        var buffer = new byte[1024];

        while (true)
        {
            var bytesLidos = stream.Read(buffer, 0, 1024);
            fs.Write(buffer, 0, bytesLidos);
            fs.Flush();
            Console.WriteLine($"Total de Bytes Lidos: {bytesLidos}");
        }

    }
}

static void UsandoAClasseFile()
{
    var linhas = File.ReadAllLines("contas.txt");
    Console.WriteLine(linhas.Length);

    var bytesArquivo = File.ReadAllBytes("contas.txt");
    Console.WriteLine($"Arquivo contas.txt possui {bytesArquivo.Length} bytes");

    File.WriteAllText("escrevendoComClasseFile.txt", "Testando File.WriteAllText");

    foreach (var linha in linhas)
    {
        Console.WriteLine(linha);
    }

    Console.WriteLine("Aplicação Finalizada ...");
}

#endregion
