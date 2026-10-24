let nextId = 3

const store = [
  {
    id: 1,
    number: 1,
    title: 'Математический анализ',
    description: 'Пропустил экзамен по болезни, есть справка',
    disciplineId: 2,
    status: 'New',
    assigneeUserId: null,
    retakeDate: null,
    retakeTime: null,
    audience: null,
  },
  {
    id: 2,
    number: 2,
    title: 'Физика',
    description: 'Нужна пересдача после комиссии',
    disciplineId: 1,
    status: 'InProgress',
    assigneeUserId: 5,
    retakeDate: '2026-09-25',
    retakeTime: '14:30',
    audience: 'А-312',
  },
]

const DELAY_MS = 500

export const force500 = { value: false }

function delay(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms))
}

function ok(status, data) {
  return { ok: true, status, data }
}

function fail(status, data) {
  return { ok: false, status, data }
}

export async function mockGetTickets() {
  await delay(DELAY_MS)
  if (force500.value) {
    return fail(500, { message: 'Учебная ошибка сервера' })
  }
  return ok(200, store.map((item) => ({ ...item })))
}

export async function mockCreateTicket(body) {
  await delay(DELAY_MS)
  if (force500.value) {
    return fail(500, { message: 'Учебная ошибка сервера' })
  }

  const title = (body.title ?? '').trim()
  if (title.length < 5 || title.length > 80) {
    return fail(400, { message: 'Заголовок: от 5 до 80 символов' })
  }

  const id = nextId
  nextId += 1
  const created = {
    id,
    number: id,
    title,
    description: (body.description ?? '').trim(),
    disciplineId: body.disciplineId,
    status: 'New',
    assigneeUserId: null,
    retakeDate: null,
    retakeTime: null,
    audience: null,
  }
  store.push(created)
  return ok(201, { ...created })
}