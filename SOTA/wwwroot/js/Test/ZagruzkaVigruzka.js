function GetPredActiveLI() {
    var d = document.getElementsByClassName("nav-link active");
    var nZad = d[0].id.substr(3);

    nZad = nZad.substr(0, nZad.length - 4);



   
    podgotovkaKSave(nZad);
}

function skr(z) {
    var PB = document.getElementById("pred");
    var SB = document.getElementById("sled");
    var coll = document.getElementById("CountZ").value;
    
    if (z  == 1) {
        PB.style.display = "none";
    }
    else {
        PB.style.display = "inline-block";
    }

    if (z == coll) {
        SB.style.display = "none";
    }
    else {
        SB.style.display = "inline-block";
    }

}

function GetPred() {
    var d = document.getElementsByClassName("nav-link active");
    var PB = document.getElementById("pred");
    var nZad = d[0].id.substr(3);
    nZad = nZad.substr(0, nZad.length - 4);
    
    nZad = nZad - 1;
    if (nZad == 1) {
        PB.style.display = "none";
    }
    else {
        PB.style.display = "inline-block";
    }
    if (document.getElementById("Zad" + nZad + "-tab") != null)
        document.getElementById("Zad" + nZad + "-tab").click();
    //Pokraska();
    podgotovkaKSave(nZad);
}

function GetSled() {
    
    var d = document.getElementsByClassName("nav-link active");
    var SB = document.getElementById("sled");
    var CL = document.getElementById("Zav");
    var coll = document.getElementById("CountZ").value;
    var nZad = d[0].id.substr(3);
    nZad = nZad.substr(0, nZad.length - 4);
    nZad = parseInt(nZad) + 1;
    if (nZad == coll) {
        SB.style.display = "none";
        CL.style.display = "inline-block";
    }
    else {
        SB.style.display = "inline-block";
        CL.style.display = "none";
    }

    if (document.getElementById("Zad" + nZad + "-tab"))
        document.getElementById("Zad" + nZad + "-tab").click();
    //Pokraska();
    podgotovkaKSave(nZad);
}

function podgotovkaKSave(nZad) {

    var id = document.getElementById("Zad" + nZad + "ID").value;

    var tip = document.getElementById("Zad" + nZad + "Tip").value;
    var text;

    if (tip == 1)
        text = document.getElementById("Zad" + nZad + "Otvet").value;
    if (tip == 2) {
        text = TextOtvTip2(nZad);
    }
    if (tip == 3) {
        text = TextOtvTip3(nZad);
    }
    if (tip == 4) {
        text = TextOtvTip4(nZad);
    }
    if (tip == 5) {
        text = document.getElementById("nZad" + nZad + "Svb").value;
    }
    console.log(text);
    if (id != null && text != "" && text != null) {

        vbd(id, text);
    }

}
function TextOtvTip2(nZad) {

    var text = "";
    for (i = 0; ; i++) {
        if (document.getElementById("nZad" + nZad + "ch" + i) != null) {
            if (document.getElementById("nZad" + nZad + "ch" + i).checked == true) {
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

            text += document.getElementById("nZad" + nZad + "tab" + i).name + "{=|" + document.getElementById("nZad" + nZad + "tab" + i).value + "|=}";


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
            data: { id: id, text: text, idRabota: idRabota }

        });
    }
    OtvVBDMass.set(parseInt(id), text);
    var d = document.getElementsByClassName("nav-link active");
    var nZad = d[0].id.substr(3);
    nZad = nZad.substr(0, nZad.length - 4);
    document.getElementById("Zad" + nZad + "-tab").style.backgroundColor = "#999999";
    console.log(OtvVBDMass);

}




function GetActiveLI() {
    var d = document.getElementsByClassName("nav-link active");


    var nZad = d[0].id.substr(3);
    nZad = nZad.substr(0, nZad.length - 4);


}
