window.ProcessDefaults = {
	tLlegada:5,
	tServicio: 10,
	prioridad: 5,
	count:1,
	getCount:function  () {
		return this.count++;
	},
	states:{
		enEspera: "En espera",
		ejecutandose: "Ejecutandose"
	}
}

function Process (t) {
	if(!t) t=0;
	this.id = ProcessDefaults.getCount();
	this.name = "Dulce "+ProcessDefaults.count;
	this.tLlegada = 0;
	this.tServicio = 0;
	this.prioridad = 0;
	this.tEjecucion = 0;
	this.state = ProcessDefaults.states.enEspera;
	//random process
	this.tLlegada =  Math.floor(t+ProcessDefaults.tLlegada*Math.random());
	this.tServicio = Math.floor(ProcessDefaults.tServicio*Math.random());
	this.prioridad = Math.floor(ProcessDefaults.prioridad*Math.random());

}
