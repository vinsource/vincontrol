google.load('maps', '3', {
    other_params: 'sensor=false'
});
google.setOnLoadCallback(
    OnGoogleLoad/// <reference path="../../Controllers/SwitchController.cs" />
);

function OnGoogleLoad() {
    InitializeChart();
    AssignEvents();
}