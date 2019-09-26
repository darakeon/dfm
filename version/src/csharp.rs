use std::fs;

use crate::version::Version;

static PATH: &str =
	r"..\site\DFM.Generic\Properties\GeneralAssemblyInfo.cs";

pub fn update_csharp(version: &Version) {
	let old_assembly = assembly_version(&version.prev);
	let new_assembly = assembly_version(&version.code);

	let old_file = assembly_file_version(&version.prev);
	let new_file = assembly_file_version(&version.code);

	let content = fs::read_to_string(PATH)
		.unwrap()
		.replace(&old_assembly, &new_assembly)
		.replace(&old_file, &new_file);

	fs::write(PATH, content).expect("error on c# recording");
}

fn assembly_version(version: &str) -> String {
	format!(r#"[assembly: AssemblyVersion("{}")]"#, version)
}

fn assembly_file_version(version: &str) -> String {
	format!(r#"[assembly: AssemblyFileVersion("{}")]"#, version)
}
