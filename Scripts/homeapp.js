var app = angular.module('gamesAngularList', []);
var gameTypes = gameModel.map(getGameTypes);
function getGameTypes(item) {
    return item.GameTypeName;
};
gameTypes = [...new Set(gameTypes)];
var gameTypes2 = gameTypes.map(function (item, index) {
    return { 'name': item, 'index': index };
});
console.log(gameModel);
app.controller('gamesAngularController', function ($scope) {
    $scope.angularModel = gameModel;
    $scope.logoTypes = {
        'Circle Flat': 'circleflat',
        'Oval Flat': 'ovalflat',
        'Rectangle': 'rectangle',
        'Square': 'square'
    };
    $scope.gameTypes = gameTypes;
    $scope.orderByThis = function (x) {
        $scope.gameOrdering = x;
    };
});