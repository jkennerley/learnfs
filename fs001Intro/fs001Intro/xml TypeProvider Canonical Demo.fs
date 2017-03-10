// ******************
#r """..\packages\FSharp.Data.2.3.2\lib\net40\FSharp.Data.dll"""
#r "System.Xml.Linq.dll"
//System.Xml.Linq

open FSharp.Data
// .................


// ******************
// demo 1 :: by cml-attributes
// this gens a type; the type called Auhtor ; has properties name and born
type AuthorAlt = XmlProvider<"""<author name="Paul Feyerabend" born="1924" />""">
// // // instance of the type author ; it has name and born with values
let doc = """<author name="Karl Popper" born="1902" />"""
let sampleAlt = AuthorAlt.Parse(doc)
printfn "%s (%d)" sampleAlt.Name sampleAlt.Born
// AuthorAlt has type Author with it. Thst Author type has name and born member. name is string, born is int
//x:AuthorAlt.
// .................



// ******************
// demo 2 :: by elements
// this gens a type; the type called Auhtor ; has properties name and born
type AuthorAlt2 = XmlProvider<"<author><name>Karl Popper</name><born>1902</born></author>">
// instance of the type author ; it has name and born with values
let doc2 = "<author><name>Paul Feyerabend</name><born>1924</born></author>"
let sampleAlt2 = AuthorAlt2.Parse(doc2)
printfn "%s (%d)" sampleAlt2.Name sampleAlt2.Born
// .................

// ******************
// demo 3
// author[full=true]/name
// cannot do author.Name text vaalue, since has to carry the full attribuet as well
// So, create a Name type as well
type DetailedType = XmlProvider<"""<author><name full="true">Karl Popper</name></author>""">
let info = DetailedType.Parse("""<author><name full="false">Thomas Kuhn</name></author>""")
// 
printfn "%s (full=%b)" info.Name.Value info.Name.Full 
// DetailedType carries an Author and a Name type
// property Full is type bool
// property Name is type Name and has property Value of type string
//DetailedType.Name 
// .................



// ******************
// demo 4 :: types for multiple sample elements
// list of Value. Those nodes are all int, so => Values int []
type Test = XmlProvider<"<root><value>1</value><value>3</value></root>">
// int array
let vs = Test.GetSample().Values
Test.GetSample().Values
    |> Seq.iter (printfn "%d")
// .................

let foo (x:int option ): string =
    match x with 
        | Some(x) -> x.ToString()
        | None -> "[NONE]"
     

// ******************
// demo 5 :: 
// 
type AuthorsType = XmlProvider<"""   
<authors topic="Philosophy of Science">
  <author name="Paul Feyerabend" born="1924" />
  <author name="Thomas Kuhn" />
</authors> 
""">
// one of the authors has extra died attribute
let authorsXmlString = """
  <authors topic="Philosophy of Mathematics">
    <author name="Bertrand Russell" />
    <author name="Ludwig Wittgenstein" born="1889" />
    <author name="Alfred North Whitehead" died="1947" />
  </authors> """
// authors has property Topic of type string
// authors has property Authors, type AuthorsType.Author
// type AuthorsType.Author{name:string Born:init option }
let authors =  AuthorsType.Parse(  authorsXmlString)

printfn "%s" authors.Topic
authors.Authors |> Seq.iter ( fun x -> printfn "%s %A" x.Name x.Born )
authors.Authors |> Seq.iter ( fun x -> printfn "%s %s" x.Name (foo (x.Born )) )


// died? the died attribute was not on the string type provider sample
// BUT was on the example parsed.
// Therefore, died is not statically typed but is available 
// author.XElement.Attribute(XName.Get("died"))
// note the fully qualified System.Xml.Linq. for the XnMae call here...
authors.Authors 
    |> Seq.iter ( fun x -> printfn "%A" ( x.XElement.Attribute(System.Xml.Linq.XName.Get("died"))) )
// .................




// ******************
// Global inference mode :: unifies all elements of the same name
type HtmlTypeDiv = XmlProvider<"""
<div id="root">
  <span>Main text</span>
  <span>Main text 2nd span</span>
  <div id="first">
    <div>Second text</div>
    <div>Second text, 2</div>
  </div>
</div>
""" , Global=true
>


let htmlEgg = HtmlTypeDiv.Parse(""" 
<div id="root">
  <span>Main text</span>
  <div id="first">
    <div>Second text</div>
  </div>
</div>
""")

// HtmlTypeDiv.
// printfn "%s" htmlEgg.Id

/// Prints the content of a <div> element
let rec printDiv (div:HtmlTypeDiv.Div) =
  div.Spans |> Seq.iter (printfn "%s")
  //div.Div |> Seq.iter printDiv
  if div.Spans.Length = 0 && div.Divs.Length = 0 then
      div.Value |> Option.iter (printfn "%s")

// Print the root <div> element with all children  
printDiv htmlEgg 
//.................





// ******************
// rss feed
type Rss = XmlProvider<"http://tomasp.net/blog/rss.aspx">
let blog = Rss.GetSample()

// Title is a property returning string 
printfn "%s" blog.Channel.Title

// Get all item nodes and print title with link
for item in blog.Channel.Items do
  printfn " - %s (%s)" item.Title item.Link



// ******************
// xml -> domain

type Sore96433TypeFromHttp = XmlProvider<"http://laws-lois.justice.gc.ca/eng/XML/SOR-96-433.xml" >
let sore96433FromHttp = Sore96433TypeFromHttp.GetSample()

//type Sore96433Type = XmlProvider<"http://laws-lois.justice.gc.ca/eng/XML/SOR-96-433.xml" , Global=true >

type SOR96433TypeFromDiscFile =   XmlProvider<""".\XmlSourceFiles\SOR-96-433.xml""",Global=true >
let sore96433FromDiscFile = SOR96433TypeFromDiscFile.GetSample()


//SOR96433TypeFromDiscFile.



//.................














