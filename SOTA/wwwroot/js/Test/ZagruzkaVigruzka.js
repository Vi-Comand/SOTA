function GetPredActiveLI() {
    var d = document.getElementsByClassName("nav-link active");




    var nZad = d[0].id.substr(3);
    nZad = nZad.substr(0, nZad.length - 4);
   // alert(nZad);
    podgotovkaKSave(nZad);
}
function podgotovkaKSave(nZad) {
    var id = document.getElementById("Zad" + nZad + "ID").value
    var tip = document.getElementById("Zad" + nZad + "Tip").value;
    var text;
    
    if (tip == 1)
        text = document.getElementById("Zad" + nZad + "Otvet").value;
    if (tip == 2)
    {
        text = TextOtvTip2(nZad);
    }
    if (id != null && text != null)
        vbd(id, text);
    
}
function TextOtvTip2(nZad) {
    var text="";
    for (i = 0; ; i++) {
        if (document.getElementById("nZad" + nZad + "ch" + i) != null ) {
            if (document.getElementById("nZad" + nZad + "ch" + i).checked == true)
            {
                text += document.getElementById("nZad" + nZad + "ch" + i).value + ";";
                alert(text);
            }
        }
        else
            break;
    }
    return text;
}


function vbd(id, text) {
  
        jQuery.ajax({
            url: '/Test/SaveOtvet/',
            type: "POST",
            dataType: "json",
            data: { id: id, text: text },
            success: function (query) {

            }

        });
    
  
}




function GetActiveLI() {
    var d = document.getElementsByClassName("nav-link active");

   
    var nZad = d[0].id.substr(3);
    nZad = nZad.substr(0, nZad.length - 4);
    alert(nZad);
    
}
