window.ProcessDefaults = {
	tLlegada:0,
	tServicio: 10,
	prioridad: 5,
	count:1,
	tInanicion: 10,
	getCount:function  () {
		return this.count++;
	},
	states:{
		enEspera: "En espera",
		ejecutandose: "Ejecutandose",
		terminadoPorSRT: "Terminado por SRT",
		terminadoPorPrioridad: "Terminado por prioridad",
		inanicion: "Terminado por inanición"
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
	this.tAtendido =0;
	this.state = ProcessDefaults.states.enEspera;
	//random process
	this.tLlegada =  Math.floor(t+ProcessDefaults.tLlegada*Math.random());
	this.tServicio = Math.floor(ProcessDefaults.tServicio*Math.random());
	this.prioridad = Math.floor(ProcessDefaults.prioridad*Math.random());
	this.tInanicion = Math.floor(ProcessDefaults.tInanicion*Math.random());
}
