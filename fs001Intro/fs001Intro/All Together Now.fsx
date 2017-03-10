open System

// ******************
// Util/Core
let toString (x:string option) =
    match x with
        Some(x) -> x
        | None -> ""

let decimalOptionToString (x:decimal option) =
    match x with
        | Some(y) -> y.ToString()
        | None -> ""


// .................


// ******************
// Dal
// .................


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
//ctx.Dbo.RegulationSection // should show in isense
//ctx.Dbo.BodyHeader// should show in isense
// ctx.Dbo.BodySection


// .................


// .................
// .................
// .................





// ******************
// DomainTypes
// .................

// ******************
// domain type ready for mapping
type DomFootnote =  {
    label: string;
    placement: string ; 
    text :string
}

type DomSection =  {
    code: string;
    text: string;
    label: string;
}

type DomBodyHeader =  {
    Code: string               
    Label: string              
    Level: int 
    Style: string              
    TitleTextCode : string     
    TitleTextValue : string    
}                              

type DomBodySection =  {
    Code: string               
    Label: string              
    Text : string              
}                              

// .................

// ******************
#r """..\packages\FSharp.Data.2.3.2\lib\net40\FSharp.Data.dll"""
#r "System.Xml.Linq.dll"
open FSharp.Data
// .................


// ******************
// SOR96433 Service XML Types 
// types ...
type SOR96433 =   XmlProvider<""".\XmlSourceFiles\SOR-96-433.xml""", Global = true >

// SOR96433 XML Data ; instance with values from the file
let sor96433 = SOR96433.GetSample()
// .................


// ******************
// SOR96433 XML Functions
let labelToString (x:SOR96433.Label) : string =
    let n  = x.Number 
    let s  = x.String
    if n.IsSome then 
        match n with
            | Some(y) -> y.ToString()
            | None -> "[NONE-NUMBER]"
    elif s.IsSome then 
        match s with
            | Some(z) -> z
            | None -> "[NONE-NUMBER]"
    else
        "[NONE]"

let labelOptionToString (x:SOR96433.Label option ) : string =
    match x with
        | Some(y) -> if y.String.IsSome then y.String.Value else "" 
        | None    -> "[NONE]"


let textToString (x:SOR96433.Text) : string =
    let v = x.Value
    match v with 
        | Some(s) -> s
        | None -> "[NONE]"

let textOptionToString (x:SOR96433.Text option ) : string =
    match x with 
        | Some(v) ->  textToString(v)
        | None    ->   "[NONE]"

let historicalNoteToString (historicalNote:SOR96433.HistoricalNote option ) : string =
    if historicalNote.IsSome   then 
        match historicalNote.Value.Value  with
            | Some(y) -> "SOME"
            | None -> "[NONE]"
    else
        "[NONE]"

let titleTextToValue (titleText:SOR96433.TitleText ) : string =
    match titleText.Value with
        | Some(v) ->  v
        | None    ->  "[NONE]"

let titleTextToCode (titleText:SOR96433.TitleText ) : string =
    match titleText.Code with
        | Some(v) ->  v
        | None    ->   "[NONE]"

let titleTextOptionToValue(titleText:SOR96433.TitleText option ) : string =
    match titleText with 
        | Some(v) ->  titleTextToValue(v)
        | None    ->   "[NONE]"

let titleTextOptionToCode(titleText:SOR96433.TitleText option ) : string =
    match titleText with 
        | Some(v) ->  titleTextToCode(v)
        | None    ->   "[NONE]"

//.................




// ******************
// DBRepo :: { DomainTypes -> Repo >  db }
// .................

// ******************
// insert domain -> db
//let insertRegulationSection ( section:DomSection ) = 
//    let regSections = ctx.Dbo.RegulationSection
//
//    let row = regSections.Create()
//    row.RegulationSectionId   <- System.Guid.NewGuid()
//    row.Number <- section.code
//    row.Name <- section.label
//    row.Description <- Some ( section.text )
//
//    ctx.SubmitUpdates()
//
//    ()

// insert record & SaveChanges
let insertBodyHeader ( bodyHeader:DomBodyHeader ) = 
    let ctxEntity = ctx.Dbo.BodyHeader

    let row = ctxEntity.Create()

    row.Code <- bodyHeader.Code
    row.Level <- bodyHeader.Level
    row.Label <- bodyHeader.Label
    row.Style  <- bodyHeader.Style
    row.TitleTextCode  <- bodyHeader.TitleTextCode
    row.TitleTextValue <- bodyHeader.TitleTextValue

    ctx.SubmitUpdates()
    ()


let insertBodyHeader ( be:DomBodySection) = 
    let ctxEntity = ctx.Dbo.BodySection

    let row = ctxEntity.Create()
    row.Code  <- be.Code
    row.Label <- be.Label
    row.Text  <- be.Text

    ctx.SubmitUpdates()
    ()




// .................




// ******************
// map : sor type to domain

//let toDomain (x:SOR96433.Section) = 
//        { 
//            code  = toString(x.Code)  ; 
//            text  = textOptionToString (x.Text) ; 
//            label = labelToString(x.Label)
//
//            x.
//        } 

let toDomainBodyHeader(x:SOR96433.Heading) : DomBodyHeader = 
    { 
        DomBodyHeader.Code = (toString(x.Code))
        DomBodyHeader.Level = x.Level
        DomBodyHeader.Label = (labelOptionToString(x.Label)) 
        DomBodyHeader.Style = (toString(x.Style))
        DomBodyHeader.TitleTextCode = (titleTextOptionToCode (x.TitleText))
        DomBodyHeader.TitleTextValue= (titleTextOptionToValue (x.TitleText))
    }

let toDomainBodySection(x:SOR96433.Section) : DomBodySection = 
    { 
        DomBodySection.Code = toString(x.Code)
        Label = labelToString(x.Label)
        Text  = textOptionToString(x.Text)
    }


// ******************

// body headings
let bodyHeadings = sor96433.Body.Headings

// # 1479
// bodyHeadings |> Seq.length

// bodyHeadings |> Seq.iter (fun (x:SOR96433.Heading) -> printfn "%s" (toString(x.Code)) )
// bodyHeadings |> Seq.iter (fun (x:SOR96433.Heading) -> printfn "%s" (historicalNoteToString (x.HistoricalNote)) )
// bodyHeadings |> Seq.iter (fun (x:SOR96433.Heading) -> printfn "%s" (labelOptionToString ( x.Label)) )
// bodyHeadings |> Seq.iter (fun (x:SOR96433.Heading) -> printfn "%i" (x.Level) )
// bodyHeadings |> Seq.iter (fun (x:SOR96433.Heading) -> printfn "%s" (toString(x.Style)) )
// bodyHeadings |> Seq.iter (fun (x:SOR96433.Heading) -> printfn "h.tt.v : %s" (titleTextOptionToValue (x.TitleText)) )
// bodyHeadings |> Seq.iter (fun (x:SOR96433.Heading) -> printfn "h.tt.code : %s" (titleTextOptionToCode(x.TitleText)) )

// map sor Body>Headings -> domain bodyHeadings
let domainBodyHeadings = 
    bodyHeadings 
    |> Seq.map toDomainBodyHeader

// dbg print
//... domainBodyHeadings |> Seq.iter (  fun x -> printfn "%A" x )
// insert body headings to db 
// domainBodyHeadings |> Seq.iter (  fun x -> insertBodyHeader x )
domainBodyHeadings |> Seq.iter insertBodyHeader 

//.................



// map sor Body>Headings -> domain bodyHeadings
let domainBodySections = 
    sor96433.Body.Sections
    |> Seq.map toDomainBodySection

domainBodySections |> Seq.iter insertBodySection



// ******************

// // body sections 
// let bodySections  = sor96433.Body.Sections
// 
// //## 1328
// bodySections  |> Seq.length

// .................

















// ******************
// xml -> domain
// let sections  =
//     sor96433.Body.Sections
//     |> Array.map ( fun (x:SOR96433.Section) -> 
//         { 
//             code  = toString(x.Code)  ; 
//             text  = textOptionToString (x.Text) ; 
//             label = labelToString(x.Label)
//         } )
// 
// // print
// sections |> Array.iter (fun x  ->  printfn  "%A"  x )
// sections |> Array.iter (fun x  ->  insertRegulationSection   x )

//.................














