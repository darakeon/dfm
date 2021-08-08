const chart = {
	format: '{point.name}: {point.y:,.2f}',

	colorNames: [
		'success', 'info', 'warning', 'danger',
		'primary', 'highlight', 'default'
	],

	colors: [],
	patterns: [],

	plotOptions: function () {
		return {
			series: {
				animation: false,
				dataLabels: {
					enabled: true,
					connectorColor: '#777',
					format: this.format
				},
				cursor: 'pointer',
				borderWidth: 3
			}
		}
	},

	responsivePlotOptions: function () {
		return {
			series: {
				animation: false,
				dataLabels: {
					format: this.format
				}
			}
		}
	},

	rule: function () {
		return {
			condition: {
				maxWidth: 500
			},
			chartOptions: {
				plotOptions: this.responsivePlotOptions()
			}
		}
	},

	getPattern: function(index, colors) {
		const p = index % Highcharts.patterns.length;

		return {
			pattern: Highcharts.merge(
				Highcharts.patterns[p],
				{ color: colors[index] }
			)
		}
	},

	init: function(decimal, thousand) {
		const style = getComputedStyle(document.documentElement);

		for (let p = 0; p < 4; p++) {
			for (let n = 0; n < this.colorNames.length; n++) {
				const color = style.getPropertyValue(
					`--${this.colorNames[n]}-${p}`
				)

				this.colors.push(color)
			}
		}

		this.patterns = this.colors.map(
			(_, index, array) => this.getPattern(index, array)
		)

		Highcharts.setOptions({
			chart: {
				style: {
					fontFamily: style.getPropertyValue('--font-general'),
				}
			},
			lang: {
				decimalPoint: decimal,
				thousandsSep: thousand,
			},
		});
	},

	draw: function(id, title, seriesName, data) {
		const chart = Highcharts.chart(
			id,
			{
				chart: { type: 'pie' },
				title: { text: title },
				colors: this.patterns,
				plotOptions: this.plotOptions(),
				series: [{ name: seriesName, data: data }],
				responsive: { rules: [this.rule()] },
			}
		)

		const that = this

		$('#patterns-enabled').click(function () {
			chart.update({
				colors: this.checked ? that.patterns : that.colors
			})
		})
	},
}
