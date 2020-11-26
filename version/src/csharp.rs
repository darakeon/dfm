use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

pub fn update_csharp(version: &Version) {
	update_csharp_file(&version, vec!["Authentication", "Authentication.csproj"]);
	update_csharp_file(&version, vec!["BusinessLogic", "BusinessLogic.csproj"]);
	update_csharp_file(&version, vec!["Email", "Email.csproj"]);
	update_csharp_file(&version, vec!["Entities", "Entities.csproj"]);
	update_csharp_file(&version, vec!["Generic", "Generic.csproj"]);
	update_csharp_file(&version, vec!["Language", "Language.csproj"]);
	update_csharp_file(&version, vec!["MVC", "MVC.csproj"]);
	update_csharp_file(&version, vec!["Tests", "BusinessLogic", "BusinessLogic.Tests.csproj"]);
	update_csharp_file(&version, vec!["Tests", "Email", "Email.Tests.csproj"]);
	update_csharp_file(&version, vec!["Tests", "Language", "Language.Tests.csproj"]);
	update_csharp_file(&version, vec!["Tests", "MVC", "MVC.Tests.csproj"]);
	update_csharp_file(&version, vec!["Tests", "Util", "Tests.Util.csproj"]);
}

fn update_csharp_file(version: &Version, file_relative: Vec<&str>) {
	let mut path = vec!["..", "site"];

	for step in file_relative {
		path.push(step);
	}

	let file = get_path(path);

	let old_assembly = assembly_version(&version.prev);
	let new_assembly = assembly_version(&version.code);

	let old_file = assembly_file_version(&version.prev);
	let new_file = assembly_file_version(&version.code);

	let content = get_content(file.clone())
		.replace(&old_assembly, &new_assembly)
		.replace(&old_file, &new_file);

	set_content(file, content)
}

fn assembly_version(version: &str) -> String {
	format!(r#"    <AssemblyVersion>{}</AssemblyVersion>"#, version)
}

fn assembly_file_version(version: &str) -> String {
	format!(r#"<FileVersion>{}</FileVersion>"#, version)
}
