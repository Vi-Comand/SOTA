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
    alert(text);
    if (id != null && text != null)
        vbd(id, text);
    
}


function vbd(id, text) {
    alert("dasd");
    
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
