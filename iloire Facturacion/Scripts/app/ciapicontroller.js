angular.module('CoffeeInvoiceApp', [])
.controller('CoffeeInvoiceCtrl', function ($scope, $http) {
	$scope.custom = {customID:21};

	$scope.getCustomer = function () {
		
		$scope.customName = "";
		$scope.title = "";
		$http.get('/api/CoffeeInvoiceApi', { params: {id:$scope.custom.customID}}).success(function (data, status, headers, config) {
			console.log(data.name);
			$scope.customName = data.name;			
		}).error(function (data, status, headers, config) {
			$scope.title = "Oops... something went wrong";
			$scope.customName = 'Not found';
		});
	};
});