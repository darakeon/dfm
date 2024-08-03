use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

pub fn update_csharp(version: &Version) {
	update_csharp_file(&version, vec!["core", "Authentication", "Authentication.csproj"]);
	update_csharp_file(&version, vec!["core", "BusinessLogic", "BusinessLogic.csproj"]);
	update_csharp_file(&version, vec!["core", "Email", "Email.csproj"]);
	update_csharp_file(&version, vec!["core", "Entities", "Entities.csproj"]);
	update_csharp_file(&version, vec!["core", "Exchange", "Exchange.csproj"]);
	update_csharp_file(&version, vec!["core", "Generic", "Generic.csproj"]);
	update_csharp_file(&version, vec!["core", "Language", "Language.csproj"]);
	update_csharp_file(&version, vec!["core", "Logs", "Logs.csproj"]);
	update_csharp_file(&version, vec!["core", "Tests", "Generic", "Generic.Tests.csproj"]);
	update_csharp_file(&version, vec!["core", "Tests", "Entities", "Entities.Tests.csproj"]);
	update_csharp_file(&version, vec!["core", "Tests", "BusinessLogic", "BusinessLogic.Tests.csproj"]);
	update_csharp_file(&version, vec!["core", "Tests", "Email", "Email.Tests.csproj"]);
	update_csharp_file(&version, vec!["core", "Tests", "Exchange", "Exchange.Tests.csproj"]);
	update_csharp_file(&version, vec!["core", "Tests", "Generic", "Generic.Tests.csproj"]);
	update_csharp_file(&version, vec!["core", "Tests", "Language", "Language.Tests.csproj"]);
	update_csharp_file(&version, vec!["core", "Tests", "Util", "Tests.Util.csproj"]);

	update_csharp_file(&version, vec!["robot", "Robot", "Robot.csproj"]);

	update_csharp_file(&version, vec!["site", "MVC", "MVC.csproj"]);
	update_csharp_file(&version, vec!["site", "Tests", "MVC", "MVC.Tests.csproj"]);

	update_csharp_file(&version, vec!["api", "API", "API.csproj"]);
	update_csharp_file(&version, vec!["api", "Tests", "API", "API.Tests.csproj"]);
}

fn update_csharp_file(version: &Version, file_relative: Vec<&str>) {
	let mut path = vec![".."];

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
