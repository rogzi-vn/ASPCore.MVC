// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

function balanceSkillPieChart(id, json = []) {
    var _label = json.map(x => x.Name);
    var _data = json.map(x => Math.round(x.Scores * 10) / 10);
    var _backgroundColors = json.map(x => x.HexColor);
    initPieChart(id, _label, _data, _backgroundColors);
}

function initPieChart(id, _labels, _data, _backgroundColors) {
    // Pie Chart Example
    var ctx = document.getElementById(id);
    var currentPieChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: _labels,
            datasets: [{
                data: _data,
                backgroundColor: _backgroundColors,
                hoverBorderColor: "rgba(234, 236, 244, 1)",
            }],
        },
        options: {
            maintainAspectRatio: false,
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
            },
            legend: {
                display: false
            },
            cutoutPercentage: 80,
        },
    });
}