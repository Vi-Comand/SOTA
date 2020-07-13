function GetPredActiveLI() {
    var d = document.getElementsByClassName("nav-link active");
 



    var nZad = d[0].id.substr(3);
    nZad = nZad.substr(0, nZad.length - 4);
 
    podgotovkaKSave(nZad);
}
function podgotovkaKSave(nZad) {

    var id = document.getElementById("Zad" + nZad + "ID").value;
   
    var tip = document.getElementById("Zad" + nZad + "Tip").value;
    var text;
    
    if (tip == 1)
        text = document.getElementById("Zad" + nZad + "Otvet").value;
    if (tip == 2)
    {
        text = TextOtvTip2(nZad);
    }
    if (tip == 3)
    {
        text = TextOtvTip3(nZad);
    }
    if (tip == 4) {
        text = TextOtvTip4(nZad);
    }
    if (tip == 5) {
        text = document.getElementById("nZad" + nZad + "Svb").value;
    }
    console.log(text);
    if (id != null && text != "" && text != null)
    {
       
        vbd(id, text);
    }
    
}
function TextOtvTip2(nZad) {

    var text="";
    for (i = 0; ; i++) {
        if (document.getElementById("nZad" + nZad + "ch" + i) != null ) {
            if (document.getElementById("nZad" + nZad + "ch" + i).checked == true)
            {
                text += document.getElementById("nZad" + nZad + "ch" + i).value + ";";
            
            }
        }
        else
            break;
    }
    return text;
}

function TextOtvTip3(nZad) {
    var text = "";
    for (i = 0; ; i++) {
        if (document.getElementById("nZad" + nZad + "rb" + i) != null) {
            if (document.getElementById("nZad" + nZad + "rb" + i).checked == true) {
                text += document.getElementById("nZad" + nZad + "rb" + i).value;
                break;
            }
        }
        else
            break;
    }
    return text;
}

function TextOtvTip4(nZad) {
    var text = "";
    for (i = 0; ; i++) {
        if (document.getElementById("nZad" + nZad + "tab" + i) != null) {
           
            text += document.getElementById("nZad" + nZad + "tab" + i).name+"{=|"+ document.getElementById("nZad" + nZad + "tab" + i).value+"|=}";
           
            
        }
        else
            break;
    }
    return text;
}



function vbd(id, text) {

    var idRabota = document.getElementById("idRabota").value;

   // alert("text " + text);
    //alert("vbd " + OtvVBDMass.get(parseInt(id)));
    if (text != OtvVBDMass.get(parseInt(id))) {
        jQuery.ajax({
            url: '/Test/SaveOtvet/',
            type: "POST",
            dataType: "json",
            data: { id: id, text: text, idRabota: idRabota}

        });
    }
    OtvVBDMass.set(parseInt(id), text);
    console.log(OtvVBDMass);

}




function GetActiveLI() {
    var d = document.getElementsByClassName("nav-link active");

   
    var nZad = d[0].id.substr(3);
    nZad = nZad.substr(0, nZad.length - 4);
   
    
}
