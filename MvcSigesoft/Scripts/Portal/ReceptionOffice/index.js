$(document).on('nifty.ready', function () {
    $('#demo-dp-range .input-daterange').datepicker({
        format: "MM dd, yyyy",
        todayBtn: "linked",
        autoclose: true,
        todayHighlight: true
    });

    	$('#demo-jstree-1').jstree({
			    'core' : {
			        'check_callback' : true
			    },
			    'plugins' : [ 'types', 'dnd' ],
			    'types' : {
			        'default' : {
			            'icon' : 'demo-pli-folder'
			        },
			        'html' : {
			            'icon' : 'demo-pli-file-html'
			        },
			        'file' : {
			            'icon' : 'demo-pli-file'
			        },
			        'jpg' : {
			            'icon' : 'demo-pli-file-jpg'
			        },
			        'zip' : {
			            'icon' : 'demo-pli-file-zip'
			        }
			
			    }
		});







	// JSON Example
	// =================================================================
	// Require JSTree
	// =================================================================
	$('#demo-jstree-json').jstree({
		'core': {
			'data': [
				'Empty Folder',
				{
					'text': 'demo',
					'state': {
						'opened': true
					},
					'children': [
						{
							'text': 'basic',
							'state': {
								'opened': true
							},
							'children': [
								{
									'text': 'index.html', 'icon': 'jstree-file'
								},
								{
									'text': 'root.json', 'icon': 'jstree-file'
								}
							]
						},
						{
							'text': 'Readme.md', 'icon': 'jstree-file'
						}
					]
				},
				{
					'text': 'dist',
					'state': {
						'opened': true
					},
					'children': [
						{
							'text': 'themes',
							'state': {
								'opened': true
							},
							'children': [
								{
									'text': 'default',
									'state': {
										'opened': true
									},
									'children': [
										{
											'text': '32px.png', 'icon': 'jstree-file'
										},
										{
											'text': '40px.png', 'icon': 'jstree-file'
										},
										{
											'text': 'style.css', 'icon': 'jstree-file'
										},
										{
											'text': 'style.min.css', 'icon': 'jstree-file'
										}
									]
								},
								{
									'text': 'default-dark',
									'children': [
										{
											'text': '32px.png', 'icon': 'jstree-file'
										},
										{
											'text': '40px.png', 'icon': 'jstree-file'
										},
										{
											'text': 'style.css', 'icon': 'jstree-file'
										},
										{
											'text': 'style.min.css', 'icon': 'jstree-file'
										}
									]
								}
							]
						},
						{
							'text': 'jstree.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.min.js', 'icon': 'jstree-file'
						},
					]
				},
				{
					'text': 'src',
					'state': {
						'opened': true
					},
					'children': [
						{
							'text': 'intro.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.changed.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.checkbox.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.conditionalselect.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.contextmenu.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.dnd.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.massload.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.search.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.sort.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.state.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.types.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.unique.js', 'icon': 'jstree-file'
						},
						{
							'text': 'jstree.wholerow.js', 'icon': 'jstree-file'
						},
						{
							'text': 'misc.js', 'icon': 'jstree-file'
						},
						{
							'text': 'outro.js', 'icon': 'jstree-file'
						},
						{
							'text': 'sample.js', 'icon': 'jstree-file'
						},
						{
							'text': 'themes', 'icon': 'jstree-file'
						},
						{
							'text': 'vakata-jstree.js', 'icon': 'jstree-file'
						}
					]
				},
				'unit',
				'visual',
				{
					'text': 'package.zip',
					'icon': 'jstree-file'
				}
			]
		}
	});



			
		
});
    

