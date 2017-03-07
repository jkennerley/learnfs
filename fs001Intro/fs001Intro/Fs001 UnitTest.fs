namespace fs001Intro

open Xunit
open Xunit.Abstractions 

type Basics( output : ITestOutputHelper ) = 

    let sq x = x * x 

    [<Fact>]
    let ``basic assert 1``() = 
        output.WriteLine("1")
        Assert.True(1 = 1)
    
    [<Fact>]
    let ``sq 2 is 4``() = 
        output.WriteLine("2")
        let sq2 = sq 2
        Assert.Equal (4,sq2)


    [<Fact>]
    let ``sequence of sq is {1;4;9}``() = 

        let sqs = Seq.map sq {1 .. 3}
        
        let xs = sqs |> Seq.fold (fun acc elem -> acc + " " + elem.ToString()  ) ""
        let ss = sprintf "%s" xs

        output.WriteLine(ss)

        Assert.Equal<int seq>( sqs , [1;4;9] )




s