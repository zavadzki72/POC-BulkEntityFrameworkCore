
# Bulk Merge EF Core

Comparando a nova funcionalidade do EFCore 7 (BulkMerge), como o metodo antigo de atualização/remoção de dados.


## Dependencias
- [.NET 6](https://dotnet.microsoft.com/download)
- [SqlServer](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [EF Core 7](https://learn.microsoft.com/en-us/ef/)
- [Docker](https://www.docker.com) *

> *Você não precisa necessáriamente ter o docker instalado, mas você consegue subir o SQLServer usando a `docker-compose.yml`.
## Rodando localmente

Clone o projeto

```bash
  git clone https://github.com/zavadzki72/POC-BulkMerge.git
```

Entre no diretório do projeto

```bash
  cd POC-BulkMerge
```

Suba o docker-compose
```bash
  docker-compose up -d
```

Atualize o banco com as migrations

```bash
  Update-Database -StartupProject BulkMerge.RunMigrations -Project BulkMerge.App -Context ApplicationContext
```
OU
```bash
  cd .\BulkMerge.RunMigrations\
  dotnet run -program BulkMerge.RunMigrations.csproj
```

Agora, é necessário criar a massa de dados que iremos utilizar para nosso teste de performance, para isso existe o projeto `BulkMerge.PopuleDatabase`, no mesmo você encontra o `appsettings.json` e dentro dele existe a quantidade de dados a serem criados

![image 1](https://user-images.githubusercontent.com/33812121/214970689-ab4bfc7f-ac58-42ba-8258-8659fe653199.png)


É importante ressaltar que os dados são inseridos de **10.000** em **10.000** até chegar na quantidade total. Caso queira mudar essa lógica, ela está no arquivo `Program.cs`

![image 2](https://user-images.githubusercontent.com/33812121/214970739-28e06167-beb2-432c-903b-8284a600ba3a.png)


> É importante freezar que como eu estava em um ambiente aonde usava o SQLServer via docker, ficou *IMPOSSIVEL* realizar testes com massa de dados maior que *500.000* registros.

Depois de ter a base com a massa de dados gerada, o proximo step é rodar o projeto e esperar a analise do Benchmark

```bash
  cd .\BulkMerge.App\
  dotnet run -program BulkMerge.App.csproj -c Release
```


## Resultados
Como resultado da analise, rodando localmente com um banco de dados dockerizados, eu tive a seguinte resposta

``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 6.0.13 (6.0.1322.58009), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.13 (6.0.1322.58009), X64 RyuJIT AVX2


```
|                         Method |        Mean |     Error |    StdDev |
|------------------------------- |------------:|----------:|----------:|
| UpdateWithoutBulkWithListOfIds | 1,936.18 ms | 18.956 ms | 15.829 ms |
|    UpdateWithBulkWithListOfIds |    64.17 ms |  0.797 ms |  0.706 ms |
|              UpdateWithoutBulk | 1,736.00 ms | 34.037 ms | 44.258 ms |
|                 UpdateWithBulk |    68.93 ms |  1.114 ms |  1.042 ms |


Caso queira, são gerados arquivos de resultado mais detalhados dentro da pasta `BenchmarkDotNet.Artifacts\results`:

![image 3](https://user-images.githubusercontent.com/33812121/214970614-161bcca1-4e68-4985-a8c3-a90d8865e81d.png)


Com esses resultados chega-se a conclusão que apesar de ainda ser algo novo e limitado (por enquanto), utilizar o BULK Merge do EFCore é **MUITO** mais performatico do que utilizar um Update/Delete convencional.
## Referências

 - [Benchmark](https://benchmarkdotnet.org)
 - [Bulk EF Core](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#executeupdate-and-executedelete-bulk-updates)



## Feedback

Se você tiver algum feedback, por favor entre em [contato](https://marccusz.com) =)
