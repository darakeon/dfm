const chart = {
	format: '{point.name}: {point.y:,.2f}',

	colorNames: [
		'success', 'info', 'warning', 'danger',
		'primary', 'highlight', 'default'
	],

	colors: [],
	patterns: [],
	style: {},

	plotOptions: function () {
		return {
			series: {
				animation: false,
				dataLabels: {
					enabled: true,
					format: this.format
				},
				cursor: 'pointer',
				borderWidth: 3,
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

	cssVar: function(name) {
		return this.style.getPropertyValue(`--${name}`)
	},

	init: function(decimal, thousand) {
		this.style = getComputedStyle(document.documentElement)

		for (let p = 0; p < 4; p++) {
			for (let n = 0; n < this.colorNames.length; n++) {
				const color = this.cssVar(
					`${this.colorNames[n]}-${p}`
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
					fontFamily: this.cssVar('--font-general'),
				},
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
				title: {
					text: title,
					style: {
						color: this.cssVar('primary-0'),
					}
				},
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
