window.renderChart = (canvasId, labels, data) => {
    var canvas = document.getElementById(canvasId);
    if (!canvas) {
        console.error("Canvas with id " + canvasId + " not found.");
        return;
    }
    var ctx = canvas.getContext('2d');
    if (window.myChart) {
        window.myChart.destroy();
    }
    window.myChart = new Chart(ctx, {
        type: 'bar', // hoặc 'line'
        data: {
            labels: labels,
            datasets: [{
                label: 'Transactions by Day',
                data: data,
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: { beginAtZero: true }
            }
        }
    });
};
