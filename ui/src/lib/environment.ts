export const Environment = {
  apiHostName: import.meta.env.VITE_API_HOSTNAME,
  apiPort: import.meta.env.VITE_API_PORT,
  apiProtocol: import.meta.env.VITE_API_PROTOCOL,
  apiImporterHostName: import.meta.env.VITE_API_IMPORTER_HOSTNAME,
  apiImporterPort: import.meta.env.VITE_API_IMPORTER_PORT,
  apiImporterProtocol: import.meta.env.VITE_API_IMPORTER_PROTOCOL,
  isDev: import.meta.env.DEV,
  isProd: import.meta.env.PROD,
  env: import.meta.env.VITE_ENV,
};
