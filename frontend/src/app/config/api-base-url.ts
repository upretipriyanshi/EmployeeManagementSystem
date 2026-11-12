declare global {
  interface Window {
    appConfig?: {
      apiBaseUrl?: string;
    };
  }
}

export const API_BASE_URL = (window.appConfig?.apiBaseUrl ?? 'http://localhost:5194').replace(
  /\/$/,
  '',
);
