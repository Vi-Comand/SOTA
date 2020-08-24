
function vbd() {

    var idRabota = document.getElementById("idRabota").value;

   // alert("text " + text);
    //alert("vbd " + OtvVBDMass.get(parseInt(id)));
    
        jQuery.ajax({
            url: '/Test/KonecRabot/',
            type: "POST",
            dataType: "json",
            data: {idRabota: idRabota}

        });
  
   

}


