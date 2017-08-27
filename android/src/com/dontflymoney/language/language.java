Locale locale = new Locale("en_US"); 
Locale.setDefault(locale);
Configuration config = new Configuration();
config.locale = locale;
context.getApplicationContext().getResources().updateConfiguration(config, null);