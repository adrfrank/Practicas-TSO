function debugPrint(msg){
	console.log(msg);
}

var app = angular.module("app",[]);
app.controller("barberiaController",["$scope",function  ($scope) {
	$scope.title = "Dulceria"
	$scope.processes = [];
	$scope.terminated = []
	$scope.maxProcesses = 10;
	$scope.fillTime = 20;
	$scope.time = 0;
	$scope.running = false;
	$scope.strProcess = null;
	$scope.priorityProcess = null;

	$scope.runStop = function(){
		$scope.running = !$scope.running;
		if($scope.running==true) $scope.time=0;

	};
	$scope.fillProcesses = function  () {
		debugPrint("Llenando procesos");
		var c = Math.max( $scope.maxProcesses - $scope.processes.length,0 );			
		for(var i=0; i < c; ++i){
			$scope.processes.push(new Process($scope.time));
		}		
	}
	$scope.timeCount=function()
	{		
		if($scope.running){
			$scope.time++;
			debugPrint($scope.time);
			if($scope.time%$scope.fillTime == 0)
				$scope.fillProcesses();
			$scope.$apply();
		}
	}
	$scope.getNextSRT=function(){
		$scope.strProcess = $scope.processes[0];
		$scope.processes.splice(0,1);
	}

	$scope.getNextPriority=function(){
		$scope.priorityProcess = $scope.processes[0];
		$scope.processes.splice(0,1);
	}

	$scope.barberoSRTCount = function(){
		if($scope.running){
			if($scope.strProcess==null && $scope.processes.length>0){
				$scope.getNextSRT();
				$scope.strProcess.tStart = $scope.time;
				$scope.strProcess.state = ProcessDefaults.states.ejecutandose;
			}else if($scope.strProcess!=null){
				if($scope.strProcess.tEjecucion < $scope.strProcess.tServicio){
					$scope.strProcess.tEjecucion++;
				}
				if($scope.strProcess.tEjecucion >= $scope.strProcess.tServicio){
					$scope.strProcess.state = ProcessDefaults.states.terminadoPorSRT;
					$scope.terminated.push($scope.strProcess);
					$scope.strProcess = null;
				}
			}
			$scope.$apply();
		}
	}

	$scope.barberoPriorityCount = function(){
		if($scope.running){
			if($scope.priorityProcess==null && $scope.processes.length>0){
				$scope.getNextPriority();
				$scope.priorityProcess.tStart = $scope.time;
				$scope.priorityProcess.state = ProcessDefaults.states.ejecutandose;
			}else if($scope.priorityProcess!=null){
				if($scope.priorityProcess.tEjecucion < $scope.priorityProcess.tServicio){
					$scope.priorityProcess.tEjecucion++;
				}
				if($scope.priorityProcess.tEjecucion >= $scope.priorityProcess.tServicio){
					$scope.priorityProcess.state = ProcessDefaults.states.terminadoPorPrioridad;
					$scope.terminated.push($scope.priorityProcess);
					$scope.priorityProcess = null;
				}
			}
			$scope.$apply();
		}
	}

	$scope.fillProcesses();
	setInterval($scope.timeCount,1000);
	setInterval($scope.barberoSRTCount,1000);
	setInterval($scope.barberoPriorityCount,1000);
}]);