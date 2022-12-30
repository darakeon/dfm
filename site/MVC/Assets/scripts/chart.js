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

		this.setFills()
		this.setOptions(decimal, thousand)
		this.setModeChanger()
	},

	setFills: function() {
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
	},

	setOptions: function (decimal, thousand) {
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

	setModeChanger: function() {
		$('#patterns-enabled').click(() => {
			this.changeAccessibilityMode()
		})

		$('#patterns-enabled').prop(
			"checked",
			this.isPatternsEnabled()
		)
	},

	changeAccessibilityMode: function () {
		const patternEnabled = $('#patterns-enabled').is(':checked')

		this.setFill(patternEnabled)

		this.charts.forEach(
			c => c.update({
				colors: this.getFill()
			})
		)
	},

	setFill: function (patternEnabled) {
		localStorage.setItem('patterns-enabled', patternEnabled ? 1 : 0)
	},

	getFill: function () {
		return this.isPatternsEnabled()
			? this.patterns
			: this.colors
	},

	isPatternsEnabled: function () {
		return localStorage.getItem('patterns-enabled') === '1'
	},

	charts: [],

	draw: function (id, title, seriesName, data, noData) {
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
				colors: this.getFill(),
				plotOptions: this.plotOptions(),
				series: [{ name: seriesName, data: data }],
				responsive: { rules: [this.rule()] },
				lang: { noData }
			}
		)

		this.charts.push(chart)
	},
}
