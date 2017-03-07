namespace fs001Intro

open Xunit
open Xunit.Abstractions 

type QuicksortUnitTest( output : ITestOutputHelper ) = 

    let sq x = x * x
    
    let rec quicksort = function 
        | [] -> []
        | x :: xs ->
            let smaller = List.filter ((>) x) xs 
            let larger = List.filter ((<=) x ) xs
            quicksort smaller @ [x] @ quicksort larger

    [<Fact>]
    let ``quicksort [1;2] is [1;2]``() =
        let xs = [1;2]
        Assert.Equal<int list> ( [1;2] , (quicksort xs) )
        
    [<Fact>]
    let ``quicksort [2;1] is [1;2]``() =
        let xs = [2;1]
        Assert.Equal<int list> ( [1;2] , (quicksort xs) )
            
    [<Fact>]
    let ``quicksort [2.;1.] is [1.;2.]``() =
        let xs = [2.;1.]
        Assert.Equal<float list> ( [1.;2.] , (quicksort xs) )
    
    [<Fact>]
    let ``quicksort [2.;1.] is [1.;2.]``() =
        let xs = ["a";"b"]
        Assert.Equal<string list> ( ["a";"b"] , (quicksort xs) )


