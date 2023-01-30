
# Bulk EF Core 7

Comparando a nova funcionalidade do EFCore 7 (Bulk), com o metodo antigo de atualização/remoção de dados.


## Dependencias
- [.NET 6](https://dotnet.microsoft.com/download)
- [SqlServer](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [EF Core 7](https://learn.microsoft.com/en-us/ef/)
- [Docker](https://www.docker.com) *
- [EF Extensions](https://entityframework-extensions.net) *

> *Você não precisa necessáriamente ter o docker instalado, mas você consegue subir o SQLServer usando a `docker-compose.yml`.

> *Você não precisa também da LIB entity framework extension, ela está presente no repositório para conseguir testar a funcionalidade MERGE.
## Rodando localmente

Clone o projeto
```bash
  git clone https://github.com/zavadzki72/POC-BulkEntityFrameworkCore.git
```

Entre no diretório do projeto
```bash
  cd POC-BulkEntityFrameworkCore
```

Suba o docker-compose
```bash
  docker-compose up -d
```

Atualize o banco com as migrations

```bash
  Update-Database -StartupProject Bulk.RunMigrations -Project Bulk.App -Context ApplicationContext
```
OU
```bash
  cd .\Bulk.RunMigrations\
  dotnet run -program Bulk.RunMigrations.csproj
```

Agora, é necessário criar a massa de dados que iremos utilizar para nosso teste de performance, para isso existe o projeto `Bulk.PopuleDatabase`, no mesmo você encontra o `appsettings.json` e dentro dele existe a quantidade de dados a serem criados

![AppSettings](https://user-images.githubusercontent.com/33812121/214970689-ab4bfc7f-ac58-42ba-8258-8659fe653199.png)


É importante ressaltar que os dados são inseridos de **10.000** em **10.000** até chegar na quantidade total. Caso queira mudar essa lógica, ela está no arquivo `Program.cs`

![ProgramCs](https://user-images.githubusercontent.com/33812121/215113583-60d21d72-5014-4293-a6e0-79204540b83d.png)

> É importante freezar que como eu estava em um ambiente local aonde usava o SQLServer via docker, ficou *IMPOSSIVEL* realizar testes com massa de dados maior que *500.000* registros.

Depois de ter a base com a massa de dados gerada, o proximo step é rodar o projeto e esperar a analise do Benchmark

```bash
  cd .\Bulk.App\
  dotnet run -program Bulk.App.csproj -c Release
```

## Projetos
Dentro da solution encontramos 4 projetos. São eles:
1. Bulk.App
2. Bulk.PopuleDatabase
3. Bulk.RunMigrations
4. Bulk.Test

##

#### Bulk.App
Esse projeto é basicamente aonde ficam as classes do _BenchmarkDotNet_(Classes de teste de performance), nesse projeto é possível alterar algumas configurações envolvidas nos testes dentro do arquivo de configuração do mesmo (`appsettings.json`) 

```json
{
      "SqlServer": "Data Source=localhost,5434;Initial Catalog=BulkTest;User ID=sa;Password=Pass@word;Connect Timeout=15;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultipleActiveResultSets=true;",
      "QuantityFilterIds": 25000,
      "QuantityFilterTeams": 500,
      "QuantityFilterTeamsInsert": 10,
      "TestBulkMerge": true,
      "TestBulk": true
}
```

**QuantityFilterIds:** Filtro utilizado no teste de **Update**, o mesmo serve para determinar a quantia de IDs a serem filtrados para realizar o update.
*Exemplo:*

![QuantityFilterIds](https://user-images.githubusercontent.com/33812121/215590002-f9df0a52-8469-412c-9fe2-5a26a0e0a1ab.png)

**QuantityFilterTeams:** Filtro utilizado no teste de **Upsert**, o mesmo serve para determinar a quantia de times a serem utilizados para realizar o merge.

**QuantityFilterTeamsInsert:** Filtro utilizado no teste de **Upsert**, o mesmo serve para determinar a quantia de times a serem utilizados para realizar o merge (Items novos).

*Exemplo:*

![QuantityFilterTeams](https://user-images.githubusercontent.com/33812121/215590376-3520fb6f-3d9e-49ff-893c-a13e68691dfe.png)

**TestBulkMerge:** Determina se o bulkmerge será testado

**TestBulk:** Determina se o bulk será testado

*Exemplo:*

![TestBulkMerge](https://user-images.githubusercontent.com/33812121/215590897-c4ad12b2-d8cc-4d09-84ec-3d9f57e23a3d.png)

##

#### Bulk.PopuleDatabase
Esse projeto tem como objetivo popular a base de dados, nesse projeto é possível alterar algumas configurações envolvidas nessa inserção dentro do arquivo de configuração do mesmo (`appsettings.json`) 
```json
{
  "SqlServer": "Data Source=localhost,5434;Initial Catalog=BulkTest;User ID=sa;Password=Pass@word;Connect Timeout=15;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultipleActiveResultSets=true;",
  "SqlServerDev": "Data Source=localhost,5434;Initial Catalog=BulkTestDev;User ID=sa;Password=Pass@word;Connect Timeout=15;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultipleActiveResultSets=true;",
  "IsDev": false,
  "QuantityItemsToInsert": 500000
}
```
> Repare que existem duas connections dentro desse AppSettings, isso serve para que possamos realizar testes de metodos utilizados antes de realizar o teste de performance.

**IsDev:** Define a base de dados a ser realizado a inserção de dados.
**QuantityItemsToInsert:** Define a quantidade total de items a serem inseridos.

##

#### Bulk.RunMigrations
Esse projeto tem como objetivo atualizar a base com a estrutura esperada (Utilizando migrations), nesse projeto é possível alterar para qual base está sendo realizada a atualização de estrutra *(Recomendo manter as duas bases com a mesma versão de migration)*
```json
{
  "SqlServer": "Data Source=localhost,5434;Initial Catalog=BulkTest;User ID=sa;Password=Pass@word;Connect Timeout=15;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultipleActiveResultSets=true;",
  "SqlServerDev": "Data Source=localhost,5434;Initial Catalog=BulkTestDev;User ID=sa;Password=Pass@word;Connect Timeout=15;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultipleActiveResultSets=true;",
  "IsDev": true
}
```

##

#### Bulk.Test
Esse projeto tem como objetivo realizar testes com uma base de dados menor, antes de realizar algum teste de performance.

## Resultados Bulk
Como resultado da analise do bulk `(PerformanceTests)`, rodando localmente com um banco de dados dockerizados, eu tive a seguinte analise

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

![BenchmarkResults](https://user-images.githubusercontent.com/33812121/215113939-fda9c952-d8f6-4728-ad67-717a923b58d1.png)

Com esses resultados chega-se a conclusão que apesar de ainda ser algo novo e limitado (Senti falta de um BulkMerge(Upsert)), utilizar o BULK do EFCore é **MUITO** mais performatico do que utilizar um Update/Delete convencional. Esperamos que conforme o tempo passe, o mesmo seja incorporado =).

## Resultados BulkMerge
Como resultado da analise do bulkmerge `(BulkMergePerformanceTests)`, rodando localmente com um banco de dados dockerizados, eu tive a seguinte analise

É importante ressaltar que para o BulkMerge foram adotadas três maneiras:
1. Utilizando o [Entity Framework Extensions](https://entityframework-extensions.net) (Uma LIB paga que tem a extensão do BulkMerge entre outras tantas).
2. Utilizando o BULK nativo (EFCore 7)
3. Utilizando o update convencional (EFCore)

#### Exemplo de utilização dos casos
##### Caso 1
```csharp
public async Task UpsertWithBulkLib(List<TeamEntity> teams)
{
    await _applicationContext.Set<TeamEntity>()
        .BulkMergeAsync(teams, x =>
        {
            x.IncludeGraph = true;
            x.IncludeGraphOperationBuilder = op =>
            {
                if(op is BulkOperation<StadiumEntity>)
                {
                    BulkOperation<StadiumEntity> bulk = (BulkOperation<StadiumEntity>)op;
                    bulk.ColumnPrimaryKeyExpression = y => y.Name;
                }
                else if (op is BulkOperation<TeamEntity>)
                {
                    BulkOperation<TeamEntity> bulk = (BulkOperation<TeamEntity>)op;
                    bulk.ColumnPrimaryKeyExpression = y => y.Name;
                }
            };
        }
    );
}
```

##### Caso 2
```csharp
public async Task UpsertWithBulk(List<TeamEntity> teams)
{
    foreach(var team in teams)
    {
        var teamInBase = await _applicationContext.Set<TeamEntity>().Where(x => x.Name.Equals(team.Name)).ToListAsync();
        if (teamInBase != null && teamInBase.Any())
        {
            await _applicationContext.Set<TeamEntity>()
                .Where(x => x.Name.Equals(team.Name))
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(x => x.Initials, team.Initials)
                    .SetProperty(x => x.Founded_At, team.Founded_At)
                    .SetProperty(x => x.Country, team.Country)
                );
        }
        else
        {
            await _applicationContext.Set<TeamEntity>()
                .AddAsync(team);
        }
    }

    await _applicationContext.SaveChangesAsync();
}
```

##### Caso 3
```csharp
public async Task UpsertWithBulk(List<TeamEntity> teams)
{
    foreach(var team in teams)
    {
        var teamInBase = await _applicationContext.Set<TeamEntity>().Where(x => x.Name.Equals(team.Name)).ToListAsync();
        if (teamInBase != null && teamInBase.Any())
        {
            teamToUpdate.Initials = team.Initials;
            teamToUpdate.Founded_At = team.Founded_At;
            teamToUpdate.Country = team.Country;

            _applicationContext.Set<TeamEntity>().Update(teamToUpdate);
        }
        else
        {
            await _applicationContext.Set<TeamEntity>()
                .AddAsync(team);
        }
    }

    await _applicationContext.SaveChangesAsync();
}
```

#### Resultados

``` ini
BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 6.0.13 (6.0.1322.58009), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.13 (6.0.1322.58009), X64 RyuJIT AVX2

```
|            Method |        Mean |      Error |       StdDev |      Median |
|------------------ |------------:|-----------:|-------------:|------------:|
| UpsertWithBulkLib |    87.42 ms |   1.742 ms |     3.438 ms |    86.05 ms |
|    UpsertWithBulk | 6,589.29 ms | 443.638 ms | 1,287.075 ms | 7,066.85 ms |
| UpsertWithoutBulk | 1,007.11 ms |  20.038 ms |    57.492 ms | 1,005.22 ms |


Caso queira, são gerados arquivos de resultado mais detalhados dentro da pasta `BenchmarkDotNet.Artifacts\results`:

![BenchmarkResults_Two](https://user-images.githubusercontent.com/33812121/215593281-dce6dccd-c40f-4e47-af03-b1b52979b928.png)

Com esses resultados chega-se a conclusão que quando falamos de BulkMerge (Merge de dados) a atualização do BULK não serve =(. Isso porque utilizando o bulk nativo do EFCore a performance é cerca de **6x** menor do que utilizando o update convencional, já a LIB Entity Framework Extensions *(que infelizmente é paga)*, se sobresai muito sendo **11x** mais performatica que o EFCore puro.

## Referências

 - [Benchmark](https://benchmarkdotnet.org)
 - [Bulk EF Core](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#executeupdate-and-executedelete-bulk-updates)
 - [Entity Framework Extensions](https://entityframework-extensions.net/bulk-update)



## Feedback

Se você tiver algum feedback, por favor entre em [contato](https://marccusz.com) =)
