// Load the Visualization API and the piechart package.
google.load('visualization', '1.0', { 'packages': ['corechart'] });

// Set a callback to run when the Google Visualization API is loaded.
google.setOnLoadCallback(drawPieChart);

// Callback that creates and populates a data table,
// instantiates the pie chart, passes in the data and
// draws it.
function drawPieChart() {
    $.get("en-US/Projects/IssueStats/SAD", function (result) {

        // Create the data table.
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Status');
        data.addColumn('number', 'Count');
        
        data.addRows(result);

        // Set chart options
        var options = {
            'height': 500,
            'chartArea': {left: 20, top: 40, width: '100%', height: '80%' },
        };

        // Instantiate and draw our chart, passing in some options.
        var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
        chart.draw(data, options);
    });
}

google.load("visualization", "1", { packages: ['corechart'] });
google.setOnLoadCallback(drawBarChart);
function drawBarChart() {
    var data = google.visualization.arrayToDataTable([
    ['Month', 'Resolved', 'Raised'],
    ['Sep', 10, 4],
    ['Oct', 11, 4],
    ['Nov', 6, 11],
    ['Dec', 10, 5]
    ]);

    var options = {
        height: 500,
        chartArea: { left: 50, top: 40, width: '70%', height: '80%' },
    };

    var chart = new google.visualization.ColumnChart(document.getElementById('bar_chart_div'));
    chart.draw(data, options);
}


$(window).resize(function () {
    drawPieChart();
    drawBarChart();
});