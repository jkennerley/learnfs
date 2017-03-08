// ******************
// Install-Package SQLProvider ## nuget
// .................

// ******************
// reference the type provider dll
#r """..\packages\SQLProvider.1.0.43\lib\FSharp.Data.SQLProvider.dll"""
open FSharp.Data.Sql
// .................

// ******************
// db model types 
let [<Literal>] dbVendor = Common.DatabaseProviderTypes.MSSQLSERVER
let [<Literal>] connString = "Data Source=.\SQLEXPRESS;Initial Catalog=jkspike; Integrated Security=True ;"
let [<Literal>] indivAmount = 1000
let useOptTypes = true
type sql = 
    SqlDataProvider<
        ConnectionString=connString, 
        DatabaseVendor=dbVendor, 
        IndividualsAmount=indivAmount,
        UseOptionTypes=true
    >
// .................

// ******************
// db ctx instance
let ctx = sql.GetDataContext()
// .................


// ******************
// add a record
let footnotes = ctx.Dbo.Footnote

let row = footnotes.Create()
row.Label<- System.Guid.NewGuid().ToString().Substring(0,4)
row.Placement<- System.Guid.NewGuid().ToString().Substring(0,4)
row.Text<- System.Guid.NewGuid().ToString().Substring(0,4)

ctx.SubmitUpdates()
// .................



// ******************
// query the db
let q1 =
    query {
        for x in ctx.Dbo.Footnote  do
        sortBy (x.Label)
        select (x)
    }
let xs = 
    q1 
    |> Seq.toArray 
    |> Array.map( fun i -> i.ColumnValues |> Map.ofSeq )
// .................



// ******************
//CREATE DATABASE jkspike ; 
//GO 
//
//USE jkspike 
//GO 
//
//--**
//DROP table [dbo].[Footnote]
//GO 
//CREATE TABLE [dbo].[Footnote]
//(
//	[Label] nvarchar(100) NOT NULL, 
//	[Placement] nvarchar(100) NOT NULL, 
//	[Text] nvarchar(100) NOT NULL
//)
//GO 
//
//INSERT INTO [dbo].[Footnote] VALUES ('ll' , 'pp' , 'te' )
//
//SELECT * FROM dbo.Footnote
// .................


// ******************
// .................
