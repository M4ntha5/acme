/// <reference types="vite/client" />

interface ImportMetaEnv {
  VITE_API_HOSTNAME?: string;
  VITE_API_PORT?: string;
  VITE_API_PROTOCOL?: string;
  VITE_API_IMPORTER_HOSTNAME?: string;
  VITE_API_IMPORTER_PORT?: string;
  VITE_API_IMPORTER_PROTOCOL?: string;
  VITE_ENV?: string;
}
