const baseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000'

export class ApiError extends Error {
  constructor(message, { status = null, kind = 'http', data = null } = {}) {
    super(message)
    this.name = 'ApiError'
    this.status = status
    this.kind = kind
    this.data = data
  }
}

function toError(status, data) {
  const message = data?.title || data?.message || `Сервер ответил статусом ${status}`
  return new ApiError(message, { status, kind: 'http', data })
}

async function handleResponse(response) {
  const data = response.headers.get('content-type')?.includes('json')
    ? await response.json()
    : null

  if (!response.ok) {
    throw toError(response.status, data)
  }
  return data
}

export async function getTickets() {
  let response
  try {
    response = await fetch(`${baseUrl}/api/retake-requests`, { credentials: 'include' })
  } catch {
    throw new ApiError('Нет связи с сервером', { kind: 'network' })
  }
  return handleResponse(response)
}

export async function createTicket(body) {
  let response
  try {
    response = await fetch(`${baseUrl}/api/retake-requests`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(body),
    })
  } catch {
    throw new ApiError('Нет связи с сервером', { kind: 'network' })
  }
  return handleResponse(response)
}

export async function login(credentials) {
  let response
  try {
    response = await fetch(`${baseUrl}/api/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(credentials),
    })
  } catch {
    throw new ApiError('Нет связи с сервером', { kind: 'network' })
  }
  return handleResponse(response)
}

export async function getCurrentUser() {
  let response
  try {
    response = await fetch(`${baseUrl}/api/auth/me`, { credentials: 'include' })
  } catch {
    throw new ApiError('Нет связи с сервером', { kind: 'network' })
  }
  return handleResponse(response)
}

export async function logout() {
  let response
  try {
    response = await fetch(`${baseUrl}/api/auth/logout`, {
      method: 'POST',
      credentials: 'include',
    })
  } catch {
    throw new ApiError('Нет связи с сервером', { kind: 'network' })
  }
  return handleResponse(response)
}