function FormText(obj) {
    if (this.ST) return; var ov = obj.value;
    var ovrl = ov.replace(/\d*\.?\d*/, '').length; this.ST = true;
    if (ovrl > 0) { obj.value = obj.lang; Fshowerror(obj); return }
    obj.lang = obj.value; this.ST = null;
}

function Fshowerror(obj) {
    if (!this.OBJ) { this.OBJ = obj; obj.style.backgroundColor = 'pink'; this.TIM = setTimeout(Fshowerror, 50) }
    else { this.OBJ.style.backgroundColor = ''; clearTimeout(this.TIM); this.ST = null; FormText(this.OBJ); this.OBJ = null }
}

