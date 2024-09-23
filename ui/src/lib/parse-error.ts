import { AxiosError } from 'axios';

export type ParsedError = {
  name: string;
  message: string;
};

export function parseError(error: unknown, errorListJoin = ', ') {
  const parsedError: ParsedError = {
    name: 'Something went wrong',
    message: error as string,
  };

  if (
    !(
      error &&
      typeof error === 'object' &&
      'name' in error &&
      'message' in error &&
      typeof error.name === 'string'
    )
  ) {
    return parsedError;
  }

  parsedError.name = error.name;
  parsedError.message = ''; // error.message as string;

  if (error instanceof AxiosError && error.isAxiosError) {
    parsedError.name = 'Error';
    parsedError.message = 'Unexpected server error';

    return parsedError;
  }

  if (
    error &&
    typeof error === 'object' &&
    'stack' in error &&
    'message' in error &&
    typeof error.message === 'string'
  ) {
    parsedError.message = error.message as string;
    return parsedError;
  }

  return parsedError;
}
