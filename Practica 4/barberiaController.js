function debugPrint(msg){
	console.log(msg);
}

var app = angular.module("app",[]);
app.controller("barberiaController",["$scope",function  ($scope) {
	$scope.title = "Dulceria"
	$scope.processes = [];
	$scope.terminated = []
	$scope.maxProcesses = 10;
	$scope.fillTime = 3;
	$scope.time = 0;
	$scope.running = false;

	$scope.runStop = function(){
		$scope.running = !$scope.running;
		if($scope.running==true) $scope.time=0;

	};
	$scope.fillProcesses = function  () {
		debugPrint("Llenando procesos");
		if($scope.running){
			var c = Math.max( $scope.maxProcesses - $scope.processes.length,0 );	
			debugPrint(c);
			for(var i=0; i < c; ++i){
				$scope.processes.push(new Process($scope.time));
			}	
					
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
	setInterval($scope.timeCount,1000);
}]);