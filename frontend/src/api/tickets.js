import { mockGetTickets, mockCreateTicket } from './mockTickets'

export const USE_MOCK = true

const baseUrl = import.meta.env.VITE_API_BASE_URL

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
  const message = data?.message || `Сервер ответил статусом ${status}`
  return new ApiError(message, { status, kind: 'http', data })
}

async function handleResponse(response) {
  let data = null
  try {
    data = await response.json()
  } catch {
    
  }

  if (!response.ok) {
    throw toError(response.status, data)
  }
  return data
}

export async function getTickets() {
  if (USE_MOCK) {
    const result = await mockGetTickets()
    if (!result.ok) throw toError(result.status, result.data)
    return result.data
  }

  let response
  try {
    response = await fetch(`${baseUrl}/api/retake-requests`)
  } catch (err) {
    throw new ApiError('Нет связи с сервером', { kind: 'network' })
  }
  return handleResponse(response)
}

export async function createTicket(body) {
  if (USE_MOCK) {
    const result = await mockCreateTicket(body)
    if (!result.ok) throw toError(result.status, result.data)
    return result.data
  }

  let response
  try {
    response = await fetch(`${baseUrl}/api/retake-requests`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body),
    })
  } catch (err) {
    throw new ApiError('Нет связи с сервером', { kind: 'network' })
  }
  return handleResponse(response)
}