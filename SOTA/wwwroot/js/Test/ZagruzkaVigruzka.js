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
    var proveren = 1;
    if (tip == 1) {
        text = document.getElementById("Zad" + nZad + "Otvet").value;
     
    }
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
        proveren = 0;
    }
    console.log(text);
    if (id != null && text != "" && text != null) {

        vbd(id, text, proveren);
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
    var per="";
    for (i = 0; ; i++) {
        if (document.getElementById("nZad" + nZad + "tab" + i) != null) {

            text += document.getElementById("nZad" + nZad + "tab" + i).name + "{=|" + document.getElementById("nZad" + nZad + "tab" + i).value + "|=}";

            per += document.getElementById("nZad" + nZad + "tab" + i).value;
        }
        else
            break;
    }


    if (per.length == 0) {
        text = '';
        
    }
 
    return text;
}



function vbd(id, text, proveren) {
  //  alert("1" + " " + id + " " + text + " " + proveren);
    var idRabota = document.getElementById("idRabota").value;

  //  alert("dase");
    //alert("vbd " + OtvVBDMass.get(parseInt(id)));
    if (text != OtvVBDMass.get(parseInt(id))) {
        jQuery.ajax({
            url: '/Test/SaveOtvet/',
            type: "POST",
            dataType: "json",
            data: { id: id, text: text, idRabota: idRabota, proveren: proveren}

        });
    }
    OtvVBDMass.set(parseInt(id), text);
    var d = document.getElementsByClassName("nav-link active");
    var nZad = d[0].id.substr(3);
    nZad = nZad.substr(0, nZad.length - 4);
    document.getElementById("Zad" + nZad + "-tab").style.backgroundColor = "#999999";

    console.log(OtvVBDMass);
    //setTimeout(
    //    () => {
    //        ChekingSafetyAnswers();
    //    },
    //    150 * 1000
    //);
}
function vbd1(id, text, proveren) {
    jQuery.ajax({
        url: '/Test/SaveOtvet/',
        type: "POST",
        dataType: "json",
        data: { id: id, text: text, idRabota: idRabota, proveren: proveren }

    });
}
var massCheking = new Array();
massCheking = [];
setInterval(() => ChekingSafetyAnswers(), 180000);
function ChekingSafetyAnswers() {
    var i = 0;

    for (let [key, value] of OtvVBDMass.entries()) {
        
        console.log(key + " " + value);


       
        if (massCheking[i] != value) {
         
            massCheking[i] = 0;
            var idRabota = document.getElementById("idRabota").value;

            jQuery.ajax({
                url: '/Test/ChekingSaveOtvet/',
                type: "POST",
                dataType: "json",
                data: { id: key, text: value, idRabota: idRabota,index:i },
                success: function (data) {
               
                
                    console.log(massCheking);
                   // alert(data.data + " " + value);
                    
                    if (data == "neok" || data.data != value) {
                        console.log(key + " " + value);
                       // vbd1(key, value, 1);

                     

                    }
                    else if (data.data == value) {
                      
                       
                        massCheking[Number(data.index)] = data.data;

                    }
                }   
            });

        }
        
    
        i++;
    }

  

    
}


 


function GetActiveLI() {
    var d = document.getElementsByClassName("nav-link active");


    var nZad = d[0].id.substr(3);
    nZad = nZad.substr(0, nZad.length - 4);


}
