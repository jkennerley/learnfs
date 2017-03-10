// ******************
#r """..\packages\FSharp.Data.2.3.2\lib\net40\FSharp.Data.dll"""
#r "System.Xml.Linq.dll"
open FSharp.Data
// .................

// ******************
// canadia : types for the SOR96433 xml file
// a type; ...
type SOR96433 =   XmlProvider<""".\XmlSourceFiles\SOR-96-433.xml""", Global = true >

// instance of the  SOR96433 file so we can get values
let sor96433 = SOR96433.GetSample()
// .................

// ******************
// sor96433  file value
//printfn "%A" sor96433.RegulationType
// printfn "%A" sor96433.Lang
// printfn "%A" sor96433.Startdate
//  printfn "%A" sor96433.Identification.Code
//  printfn "%A" sor96433.Identification.HasPreviousVersion
//  //
//  printfn "%A" sor96433.Identification.InstrumentNumber
//  //
//  printfn "%A" sor96433.Identification.LimsAuthority.Alpha
//  printfn "%A" sor96433.Identification.LimsAuthority.AuthorityTitle
//  //
//  printfn "%A" sor96433.Identification.RegistrationDate.Date.Yyyy
//  printfn "%A" sor96433.Identification.RegistrationDate.Date.Mm
//  printfn "%A" sor96433.Identification.RegistrationDate.Date.Dd
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
// .................


// ******************
let toString (x:string option) =
    match x with
        Some(x) -> x
        | None -> ""

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

let textToString (x:SOR96433.Text) : string =
    let v = x.Value
    match v with 
        | Some(s) -> s
        | None -> "[NONE]"

let textOptionToString (x:SOR96433.Text option ) : string =
    match x with 
        | Some(v) ->  textToString(v)
        | None    ->   "[NONE]"


//.................


// ******************
// xml -> 
let bodyHeadings = sor96433.Body.Headings
let bodySections = sor96433.Body.Sections

bodySections  |> Array.iter (fun (x:SOR96433.Section)  ->  printfn  "%A"  x.A )
bodySections  |> Array.iter (fun (x:SOR96433.Section)  ->  printfn  "%A"  x.AmendedText )
bodySections  |> Array.iter (fun (x:SOR96433.Section)  ->  printfn  "%A"  x.Code )



//.................

// ******************
// xml -> domain
let footnotes =
    sor96433.Order.Provision.Footnotes
    //|> Array.map ( fun (x:SOR96433.Footnote) -> { DomFootnote.label = labelToString( x.Label)  } )
    //|> Array.map ( 
    //    fun (x:SOR96433.Footnote) -> 
    //        { DomFootnote.label = labelToString( x.Label)  ; placement = x.Placement  } 
    //    )
    |> Array.map ( 
        fun (x:SOR96433.Footnote) ->  { 
                DomFootnote.label = labelToString( x.Label)  ; 
                placement = x.Placement  ;  
                text = textToString (x.Text);
            } 
        )

// print
footnotes  |> Array.iter (fun x  ->  printfn  "%s %s %s"  x.label x.placement x.text  )

// print
footnotes  |> Array.iter (fun x  ->  printfn  "%s %s %s"  x.label x.placement x.text  )





// map : sor type to domain
let toDomain (x:SOR96433.Section) = 
        { 
            code  = toString(x.Code)  ; 
            text  = textOptionToString (x.Text) ; 
            label = labelToString(x.Label)

            x.
        } 



//.................




// ******************
// xml -> domain
//let sections  = sor96433.Body.Sections |> Array.map ( fun (x:SOR96433.Section) -> toDomain x )
let sections  =
    sor96433.Body.Sections
    |> Array.map ( toDomain  )

// print
sections
    |> Array.iter (fun x  ->  printfn  "%A"  x )
//.................

