// ******************
#r """C:\Users\John\jkcentrik\learnfs\fs001Intro\packages\FSharp.Data.2.3.2\lib\net40\FSharp.Data.dll"""
#r "System.Xml.Linq.dll"
open FSharp.Data
// .................


// // ******************
// // demo 1
// // this gens a type; the type called Auhtor ; has properties name and born
// type AuthorAlt = XmlProvider<"<author><name>Karl Popper</name><born>1902</born></author>">
// // // instance of the type author ; it has name and born with values
// let doc = "<author><name>Paul Feyerabend</name><born>1924</born></author>"
// let sampleAlt = AuthorAlt.Parse(doc)
// printfn "%s (%d)" sampleAlt.Name sampleAlt.Born
// // .................


// ******************
// demo canadia
// a type; ...
type SOR96433Type =   XmlProvider<"""C:\Users\John\jkcentrik\learnfs\fs001Intro\fs001Intro\XmlSourceFiles\SOR-96-433.xml""">
// instance
let sor96433 = SOR96433Type.Load("""C:\Users\John\jkcentrik\learnfs\fs001Intro\fs001Intro\XmlSourceFiles\SOR-96-433.xml""")
// .................


// ******************
// sor96433  file value
let RegulationType = sor96433.RegulationType
printfn "%A" sor96433.RegulationType
//// // printfn "%A" sor96433.Lang
//// // printfn "%A" sor96433.Startdate
//// // 
//// //  //
//// //  printfn "%A" sor96433.Identification.Code
//// //  printfn "%A" sor96433.Identification.HasPreviousVersion
//// //  //
//// //  printfn "%A" sor96433.Identification.InstrumentNumber
//// //  // 
//// //  printfn "%A" sor96433.Identification.LimsAuthority.Alpha
//// //  printfn "%A" sor96433.Identification.LimsAuthority.AuthorityTitle
//// //  // 
//// //  printfn "%A" sor96433.Identification.RegistrationDate.Date.Yyyy
//// //  printfn "%A" sor96433.Identification.RegistrationDate.Date.Mm
//// //  printfn "%A" sor96433.Identification.RegistrationDate.Date.Dd
// .................


// ******************
// domain type ready for mapping
type DomFootnote =  { 
    label: string  ;  
    placement:string
    text :string
}
// .................


// ******************
// xml -> domain
let footnotesMap2 = 
    sor96433.Order.Provision.Footnotes 
    |> Array.map ( fun (x:SOR96433Type.Footnote) -> { label = x.Label ; placement=x.Placement ; text =x.Text } ) 
// print
footnotesMap2  
    |> Array.iter (fun x  ->  printfn  "%s %s %s"  x.label x.placement x.text  )

//.................
