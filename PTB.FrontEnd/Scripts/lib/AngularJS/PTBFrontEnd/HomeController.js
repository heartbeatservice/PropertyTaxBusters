PTBApp.controller("HomeController", function ($scope, $http, $modal) {
    $scope.section = '';
    $scope.lot = '';
    $scope.block = '';
    $scope.PageSize = 10;
    $scope.PageNumber = 1;
    $scope.propertiesData = [];
    $scope.isMainLoadingImageCollapsed = true;
    $scope.onClickFirst = function () {
        $scope.PageNumber = 1;
        $scope.onPropertySearch();
    }
    $scope.onClickLast = function () {
        $scope.PageNumber = Math.ceil($scope.LastPage);
        $scope.onPropertySearch();
    }
    $scope.onClickNext = function () {
        $scope.PageNumber = $scope.PageNumber + 1;
        $scope.onPropertySearch();
    }
    $scope.onClickPrevious = function () {
        $scope.PageNumber = $scope.PageNumber - 1;
        $scope.onPropertySearch();
    }
    $scope.onPropertySearch = function () {
        var data = {
            PageSize: $scope.PageSize,
            PageNumber: $scope.PageNumber,
            Lot: $scope.lot,
            section: $scope.section,
            block: $scope.block
        };
        $("#btnSearch").attr("class", "btn btn-primary btn-sm disabled");
        $scope.isMainLoadingImageCollapsed = false;
        var resource = location.protocol + "//" + location.host + "/api/Search/PropertySearch";
        $http.post(resource, data).success(function (data, status) {
            $scope.propertiesData = data;
            if (data.Properties.length > 0) {
                $scope.haveRows = true;
                $scope.isSuccess = false;
                $scope.isMainLoadingImageCollapsed = true;
                $scope.FirstRecord = (($scope.PageNumber - 1) * $scope.PageSize) + 1;
                $scope.LastRecord = $scope.PageNumber * $scope.PageSize;
                $scope.LastPage = $scope.propertiesData.TotalResultCount / $scope.PageSize;
                if ($scope.propertiesData.TotalResultCount < $scope.LastRecord)
                    $scope.LastRecord = $scope.propertiesData.TotalResultCount;

                $("#pagingFirst").attr("class", "paginate_button first");
                $("#pagingPrevious").attr("class", "paginate_button previous");
                $("#pagingNext").attr("class", "paginate_button next");
                $("#pagingLast").attr("class", "paginate_button last");
                if ($scope.PageNumber == 1)
                    $("#pagingPrevious").attr("class", "disabled");
                if (Math.ceil($scope.LastPage) == $scope.PageNumber)
                    $("#pagingNext").attr("class", "disabled");
                if (Math.ceil($scope.LastPage) == 1)
                {
                    $("#pagingFirst").attr("class", "disabled");
                    $("#pagingLast").attr("class", "disabled");
                }
            }
            else {
                $scope.haveRows = false;
                $scope.isMainLoadingImageCollapsed = true;
                $scope.isSuccess = true;
                $scope.successMessage = "No Record(s) found.";
            }
            $("#btnSearch").attr("class", "btn btn-primary btn-sm");
        })
         .error(function (data, status) {
             // this isn't happening:
             $scope.isMainLoadingImageCollapsed = true;
             $scope.isSuccess = true;
             $scope.successMessage = "No Record(s) found.";
             $("#btnSearch").attr("class", "btn btn-primary btn-sm");
         })
    }
    $scope.viewDetail = function (id) {
        $scope.propertyDetail = {};
        $scope.propertyDetail.Id = id;

        var modalInstance = $modal.open({
            templateUrl: 'viewDetail.html',
            controller: 'ViewDetailModalInstanceCtrl',
            size: '',
            resolve: {
                propertyDetail: function () {
                    return $scope.propertyDetail;
                },
                UpdateItem: function () {
                    return $scope.updatePropertyItem;
                }
            }
        });

        modalInstance.result.then(function (updatePropertyItemResponse) {
            //$log.info(updatePropertyItemResponse);
            if (updatePropertyItemResponse.hasOwnProperty('isError') && updatePropertyItemResponse.isError === true) {
                //showSucessMessageBox(saveCatalogResponse.message, "failure");
            }
            else {
                $scope.updatePropertyItem.Id = updatePropertyItemResponse.updatePropertyItem.Id;
                //showSucessMessageBox(saveCatalogResponse.message, "success");
            }

        }, function () {
            //$log.info('Modal dismissed at: ' + new Date());
        });
    }
    $scope.deleteItem = function (id) {
        var resource = location.protocol + "//" + location.host + "/api/Search/deleteProperty";
        $http.post(resource, id).success(function (data, status) {
            if (data == "true") {
                $scope.isSuccess = true;
                $scope.successMessage = "Record deleted successfully.";
                $scope.onPropertySearch();
            }
        })
    }
});

PTBApp.controller('ViewDetailModalInstanceCtrl', function ($scope, $http, $modalInstance, propertyDetail, UpdateItem) {

    $scope.modalTitle = "Update Property Item";

    var updatePropertyItemResponse = new Object();
    updatePropertyItemResponse.isError = false;
    updatePropertyItemResponse.message = '';
    updatePropertyItemResponse.updatePropertyItem = new Object();

    $scope.updateItem = UpdateItem;
    var Id = propertyDetail.Id;

    if (Id === null || Id === 0)
        $scope.modalTitle = "Update Property Item";

    var resource = location.protocol + "//" + location.host + "/api/Search/getPropertyById";
    $http.post(resource, Id).success(function (data, status) {
        $scope.propertyData = data;
        if ($scope.propertyData.PropertyAppealValues.length > 0) {
            $scope.haveAppealRows = true;
        }
        if ($scope.propertyData.PropertyAssessedValues.length > 0) {
            $scope.haveAssessedRows = true;
        }
    })

    $scope.UpdateItem = function () {
        var resource = location.protocol + "//" + location.host + "/api/Search/updateProperty";
        $http.post(resource, $scope.propertyData).success(function (data, status) {
            if (data == "true") {
                $scope.isSuccess = true;
                $scope.successMessage = "Record updated successfully.";
                $scope.onPropertySearch();
            }
        })
    };
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
});