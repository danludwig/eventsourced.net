import { reduxForm } from 'redux-form'

const form = 'logoff'
const fields = ['returnUrl']
const config = { form, fields, }

const Form = ({ username, handleSubmit, submitting }) => (
  <form method="post" onSubmit={handleSubmit}>
    <ul className="nav navbar-nav navbar-right">
      <li>
        <a href="#/not-implemented" title="Manage">Hello {username}!</a>
      </li>
      <li>
        <button type="submit" className="btn btn-link navbar-btn navbar-link" disabled={submitting}>Log off</button>
      </li>
    </ul>
  </form>
)

Form.propTypes = {
  username: React.PropTypes.string.isRequired,
  handleSubmit: React.PropTypes.func.isRequired,
  submitting: React.PropTypes.bool.isRequired,
  formName: React.PropTypes.string.isRequired,
}

const select = () => ({ formName: form, })
export default reduxForm(config, select)(Form)
